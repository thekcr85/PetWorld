using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Interfaces.Services;

namespace PetWorld.Infrastructure.Services;

/// <summary>
/// Simple Writer-Critic AI implementation using OpenAI HTTP API.
/// Expects OpenAI API key in configuration: OpenAI:ApiKey and optional OpenAI:ModelName.
/// </summary>
public sealed class AiChatService : IAiChatService
{
    private readonly string _apiKey;
    private readonly string _model;
    private readonly HttpClient _httpClient;
    private const int MaxIterations = 3;

    public AiChatService(IConfiguration configuration)
    {
        _apiKey = configuration["OpenAI:ApiKey"] ?? string.Empty;
        _model = configuration["OpenAI:ModelName"] ?? "gpt-4o";

        if (string.IsNullOrWhiteSpace(_apiKey))
            throw new InvalidOperationException("OpenAI:ApiKey not configured. Set it in .env or appsettings.json");

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<(string Answer, int IterationCount)> GetAnswerAsync(string question, IEnumerable<Product> products)
    {
        var productsText = FormatProducts(products);
        string answer = string.Empty;

        for (int iteration = 1; iteration <= MaxIterations; iteration++)
        {
            answer = await WriterGenerateAsync(question, productsText, answer, iteration);
            var (approved, _) = await CriticEvaluateAsync(question, answer);
            if (approved)
                return (answer, iteration);
        }

        return (answer, MaxIterations);
    }

    private async Task<string> WriterGenerateAsync(string question, string productsText, string previousAnswer, int iteration)
    {
        var systemPrompt = "Jesteœ ekspertem PetWorld. Pomagasz klientom wybieraæ produkty dla zwierz¹t. Odpowiadaj po polsku, profesjonalnie, polecaj konkretne produkty z cenami.";

        var messages = new List<object>
        {
            new { role = "system", content = systemPrompt },
            new { role = "system", content = $"Dostêpne produkty:\n{productsText}" }
        };

        if (iteration > 1 && !string.IsNullOrEmpty(previousAnswer))
        {
            messages.Add(new { role = "system", content = "Popraw swoj¹ poprzedni¹ odpowiedŸ:" });
            messages.Add(new { role = "assistant", content = previousAnswer });
        }

        messages.Add(new { role = "user", content = question });

        var payload = new
        {
            model = _model,
            messages = messages,
            temperature = 0.2
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
            throw new InvalidOperationException($"OpenAI request failed: {resp.StatusCode} {body}");

        using var doc = JsonDocument.Parse(body);
        var text = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
        return text.Trim();
    }

    private async Task<(bool Approved, string Feedback)> CriticEvaluateAsync(string question, string answer)
    {
        var systemPrompt = "Oceñ odpowiedŸ: poprawna, profesjonalna, z produktami. Zwróæ JSON: {\"approved\": true/false, \"feedback\": \"...\"}";
        var messages = new List<object>
        {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = $"Pytanie: {question}\n\nOdpowiedŸ: {answer}" }
        };

        var payload = new { model = _model, messages = messages, temperature = 0 };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
            return (true, string.Empty); // fail-open

        try
        {
            using var doc = JsonDocument.Parse(body);
            var criticText = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
            var evaluation = JsonSerializer.Deserialize<CriticResult>(criticText.Trim(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (evaluation is null)
                return (true, string.Empty);
            return (evaluation.Approved, evaluation.Feedback ?? string.Empty);
        }
        catch
        {
            return (true, string.Empty);
        }
    }

    private static string FormatProducts(IEnumerable<Product> products)
    {
        var sb = new StringBuilder();
        foreach (var p in products)
            sb.AppendLine($"- {p.Name} ({p.Category}) - {p.Price} z³ - {p.Description}");
        return sb.ToString();
    }

    private sealed class CriticResult
    {
        public bool Approved { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}
