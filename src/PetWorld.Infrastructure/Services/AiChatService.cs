using Microsoft.Agents.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Interfaces.Services;
using System.Text;
using System.Text.Json;

namespace PetWorld.Infrastructure.Services;

/// <summary>
/// AI Chat Service using Microsoft Agent Framework.
/// Implements Writer-Critic pattern: Writer generates → Critic evaluates → Iterate (max 3x).
/// </summary>
public sealed class AiChatService : IAiChatService
{
	private readonly AIAgent _writerAgent;
	private readonly AIAgent _criticAgent;
	private const int MaxIterations = 3;

	public AiChatService(IConfiguration configuration)
	{
		var apiKey = configuration["OpenAI:ApiKey"]
			?? throw new InvalidOperationException("OpenAI API Key not configured in appsettings.json");
		var modelName = configuration["OpenAI:ModelName"] ?? "gpt-4o-mini";

		// Create OpenAI client
		var openAiClient = new OpenAIClient(apiKey);
		var chatClient = openAiClient.GetChatClient(modelName);

		// Writer Agent - generates customer-facing responses with product recommendations
		_writerAgent = chatClient.AsAIAgent(
			name: "ProductWriter",
			instructions: """
                Jesteś ekspertem PetWorld - sklepu ze zwierzętami.
                Pomagasz klientom wybierać produkty dla ich pupili.
                
                ZASADY:
                - Odpowiadaj ZAWSZE po polsku
                - Bądź profesjonalny i pomocny
                - ZAWSZE polecaj konkretne produkty z nazwą i ceną
                - Jeśli klient pyta o konkretną kategorię, polecaj produkty z tej kategorii
                - Uzasadnij swoje rekomendacje
                """);

		// Critic Agent - evaluates response quality and gives feedback
		_criticAgent = chatClient.AsAIAgent(
			name: "ResponseCritic",
			instructions: """
                Jesteś krytycznym recenzentem odpowiedzi eksperta PetWorld.
                
                OCEŃ ODPOWIEDŹ według kryteriów:
                1. Czy zawiera konkretne produkty z nazwami i cenami?
                2. Czy odpowiada na pytanie klienta?
                3. Czy jest profesjonalna i pomocna?
                4. Czy rekomendacje są uzasadnione?
                
                ZWRÓĆ TYLKO I WYŁĄCZNIE JSON w formacie:
                {
                  "approved": true,
                  "feedback": ""
                }
                
                LUB jeśli odpowiedź nie spełnia kryteriów:
                {
                  "approved": false,
                  "feedback": "konkretne uwagi do poprawy"
                }
                
                NIE dodawaj żadnego tekstu poza JSON!
                """);
	}

	public async Task<(string Answer, int IterationCount)> GetAnswerAsync(
		string question,
		IEnumerable<Product> products)
	{
		var productList = FormatProducts(products);
		var answer = string.Empty;
		var criticFeedback = string.Empty;

		// Writer-Critic loop (max 3 iterations)
		for (int iteration = 1; iteration <= MaxIterations; iteration++)
		{
			// Step 1: Writer generates or improves answer
			answer = await GenerateAnswerAsync(question, productList, criticFeedback);

			// Step 2: Critic evaluates the answer
			var (isApproved, feedback) = await EvaluateAnswerAsync(question, answer);

			// Step 3: If approved, return immediately with iteration count
			if (isApproved)
			{
				return (answer, iteration);
			}

			// Store feedback for next iteration
			criticFeedback = feedback;
		}

		// Max iterations reached - return last answer
		return (answer, MaxIterations);
	}

	/// <summary>
	/// Writer Agent generates or improves answer based on critic feedback.
	/// </summary>
	private async Task<string> GenerateAnswerAsync(
		string question,
		string productList,
		string? criticFeedback)
	{
		var prompt = new StringBuilder();
		prompt.AppendLine("=== DOSTĘPNE PRODUKTY ===");
		prompt.AppendLine(productList);
		prompt.AppendLine();
		prompt.AppendLine("=== PYTANIE KLIENTA ===");
		prompt.AppendLine(question);

		// If this is iteration 2 or 3, include critic's feedback
		if (!string.IsNullOrEmpty(criticFeedback))
		{
			prompt.AppendLine();
			prompt.AppendLine("=== UWAGI DO POPRAWY ===");
			prompt.AppendLine(criticFeedback);
			prompt.AppendLine();
			prompt.AppendLine("Popraw swoją poprzednią odpowiedź uwzględniając powyższe uwagi.");
		}

		// Call Writer Agent using Microsoft Agent Framework
		var response = await _writerAgent.RunAsync(prompt.ToString());
		return response.Text.Trim();
	}

	/// <summary>
	/// Critic Agent evaluates answer quality.
	/// </summary>
	private async Task<(bool IsApproved, string Feedback)> EvaluateAnswerAsync(
		string question,
		string answer)
	{
		var evaluationPrompt = $"""
            === PYTANIE KLIENTA ===
            {question}
            
            === ODPOWIEDŹ DO OCENY ===
            {answer}
            
            Oceń powyższą odpowiedź i zwróć JSON.
            """;

		// Call Critic Agent using Microsoft Agent Framework
		var response = await _criticAgent.RunAsync(evaluationPrompt);
		return ParseCriticResponse(response.Text);
	}

	/// <summary>
	/// Parses Critic's JSON response to extract approval status and feedback.
	/// </summary>
	private static (bool IsApproved, string Feedback) ParseCriticResponse(string responseText)
	{
		try
		{
			// Remove markdown code blocks if AI wraps JSON in ```json ... ```
			var cleanJson = responseText
				.Replace("```json", "")
				.Replace("```", "")
				.Trim();

			var result = JsonSerializer.Deserialize<CriticEvaluation>(
				cleanJson,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			return (result?.Approved ?? true, result?.Feedback ?? string.Empty);
		}
		catch (JsonException)
		{
			// If JSON parsing fails, approve by default (fail-safe)
			// This prevents the system from getting stuck
			return (true, string.Empty);
		}
	}

	/// <summary>
	/// Formats product list for AI prompt.
	/// </summary>
	private static string FormatProducts(IEnumerable<Product> products)
	{
		return string.Join("\n", products.Select(p =>
			$"• {p.Name} ({p.Category}) - {p.Price:0.##} zł - {p.Description}"));
	}

	/// <summary>
	/// Critic's evaluation response model.
	/// </summary>
	private sealed class CriticEvaluation
	{
		public bool Approved { get; set; }
		public string Feedback { get; set; } = string.Empty;
	}
}