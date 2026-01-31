using PetWorld.Application.DTOs;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Interfaces.Repositories;
using PetWorld.Domain.Interfaces.Services;

namespace PetWorld.Application.Services;

/// <summary>
/// Chat service implementation.
/// Orchestrates repositories and AI service (Dependency Inversion).
/// </summary>
public sealed class ChatService : IChatService
{
	private readonly IProductRepository _productRepository;
	private readonly IChatHistoryRepository _chatHistoryRepository;
	private readonly IAiChatService _aiChatService;

	/// <summary>
	/// Constructor - dependencies injected by DI container.
	/// </summary>
	public ChatService(
		IProductRepository productRepository,
		IChatHistoryRepository chatHistoryRepository,
		IAiChatService aiChatService)
	{
		_productRepository = productRepository;
		_chatHistoryRepository = chatHistoryRepository;
		_aiChatService = aiChatService;
	}

	/// <inheritdoc />
	public async Task<ChatResponseDto> ProcessQuestionAsync(ChatRequestDto request)
	{
		// 1. Get all products (for AI recommendations)
		var products = await _productRepository.GetAllAsync();

		// 2. Process with Writer-Critic AI
		var (answer, iterationCount) = await _aiChatService.GetAnswerAsync(
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

		await _chatHistoryRepository.AddAsync(chatHistory);

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
		var history = await _chatHistoryRepository.GetAllAsync();

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