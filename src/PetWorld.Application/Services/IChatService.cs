using PetWorld.Application.DTOs;

namespace PetWorld.Application.Services;

/// <summary>
/// Chat service - main business logic for the application.
/// Orchestrates: Product retrieval → AI processing → History storage.
/// </summary>
public interface IChatService
{
	/// <summary>
	/// Processes customer question (main use case).
	/// 
	/// Workflow:
	/// 1. Get all products from repository
	/// 2. Call AI service (Writer-Critic workflow)
	/// 3. Save to chat history
	/// 4. Return response
	/// </summary>
	/// <param name="request">Customer question</param>
	/// <returns>AI answer with iteration count</returns>
	Task<ChatResponseDto> ProcessQuestionAsync(ChatRequestDto request);

	/// <summary>
	/// Gets all chat history for History page DataGrid.
	/// </summary>
	/// <returns>List of all conversations ordered by date descending</returns>
	Task<IEnumerable<ChatHistoryDto>> GetHistoryAsync();
}
