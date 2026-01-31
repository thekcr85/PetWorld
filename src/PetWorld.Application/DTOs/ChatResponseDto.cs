namespace PetWorld.Application.DTOs;

/// <summary>
/// Response DTO for chat page.
/// Returned from ChatService to Blazor component.
/// </summary>
public sealed record ChatResponseDto
{
	/// <summary>
	/// AI-generated answer (approved by Critic).
	/// </summary>
	public string Answer { get; init; } = string.Empty;

	/// <summary>
	/// Number of Writer-Critic iterations (1-3).
	/// Displayed on chat page per requirements.
	/// </summary>
	public int IterationCount { get; init; }

	/// <summary>
	/// When the conversation was created.
	/// </summary>
	public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}