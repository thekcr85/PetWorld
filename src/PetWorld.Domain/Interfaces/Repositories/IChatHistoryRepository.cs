using PetWorld.Domain.Entities;

namespace PetWorld.Domain.Interfaces.Repositories;

/// <summary>
/// Repository for storing and retrieving chat conversations.
/// Used to save Writer-Critic results and display history page.
/// </summary>
public interface IChatHistoryRepository
{
	/// <summary>
	/// Gets all chat history ordered by date descending (newest first).
	/// Used by History page DataGrid.
	/// </summary>
	/// <returns>List of all chat conversations</returns>
	Task<IEnumerable<ChatHistory>> GetAllAsync();

	/// <summary>
	/// Saves a new chat conversation to the database.
	/// Called after Writer-Critic workflow completes.
	/// </summary>
	/// <param name="chatHistory">Chat conversation to save</param>
	/// <returns>Saved chat history with generated ID</returns>
	Task<ChatHistory> AddAsync(ChatHistory chatHistory);
}