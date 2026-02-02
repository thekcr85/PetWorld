using PetWorld.Application.DTOs;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Interfaces.Repositories;
using PetWorld.Domain.Interfaces.Services;

namespace PetWorld.Application.Services;

/// <summary>
/// Chat service implementation.
/// Orchestrates repositories and AI service (Dependency Inversion).
/// </summary>
/// <param name="productRepository">Repository for product data access</param>
/// <param name="chatHistoryRepository">Repository for chat history data access</param>
/// <param name="aiChatService">AI service implementing Writer-Critic pattern</param>
public sealed class ChatService(
	IProductRepository productRepository,
	IChatHistoryRepository chatHistoryRepository,
	IAiChatService aiChatService) : IChatService
{
	/// <inheritdoc />
	public async Task<ChatResponseDto> ProcessQuestionAsync(ChatRequestDto request)
	{
		// 1. Get all products (for AI recommendations)
		var products = await productRepository.GetAllAsync();

		// 2. Process with Writer-Critic AI
		var (answer, iterationCount) = await aiChatService.GetAnswerAsync(
			request.Question,
			products);

		// 3. Save to history
		var chatHistory = new ChatHistory
		{
			Question = request.Question,
			Answer = answer,
			IterationCount = iterationCount,
			CreatedAt = DateTime.UtcNow
		};

		await chatHistoryRepository.AddAsync(chatHistory);

		// 4. Return DTO
		return new ChatResponseDto
		{
			Answer = answer,
			IterationCount = iterationCount,
			CreatedAt = chatHistory.CreatedAt
		};
	}

	/// <inheritdoc />
	public async Task<IEnumerable<ChatHistoryDto>> GetHistoryAsync()
	{
		// 1. Get all chat history from repository
		var history = await chatHistoryRepository.GetAllAsync();

		// 2. Map to DTOs (manual mapping - simple and clear)
		return history.Select(h => new ChatHistoryDto
		{
			Id = h.Id,
			Date = h.CreatedAt,
			Question = h.Question,
			Answer = h.Answer,
			IterationCount = h.IterationCount
		});
	}
}