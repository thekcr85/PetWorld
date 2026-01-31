using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Interfaces.Repositories;
using PetWorld.Infrastructure.Data;

namespace PetWorld.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of chat history repository.
/// </summary>
public sealed class ChatHistoryRepository(PetWorldDbContext context) : IChatHistoryRepository
{
	public async Task<IEnumerable<ChatHistory>> GetAllAsync()
	{
		return await context.ChatHistories
			.AsNoTracking()
			.OrderByDescending(c => c.CreatedAt)
			.ToListAsync();
	}

	public async Task<ChatHistory> AddAsync(ChatHistory chatHistory)
	{
		context.ChatHistories.Add(chatHistory);
		await context.SaveChangesAsync();
		return chatHistory;
	}
}
