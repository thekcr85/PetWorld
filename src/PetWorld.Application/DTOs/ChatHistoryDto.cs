namespace PetWorld.Application.DTOs;

/// <summary>
/// DTO for displaying chat history in DataGrid.
/// Maps from ChatHistory entity with specific formatting.
/// </summary>
public sealed record ChatHistoryDto
{
	/// <summary>
	/// Unique identifier.
	/// </summary>
	public int Id { get; init; }

	/// <summary>
	/// Conversation date (for "Data" column in DataGrid).
	/// </summary>
	public DateTime Date { get; init; }

	/// <summary>
	/// Customer question (for "Pytanie" column).
	/// </summary>
	public string Question { get; init; } = string.Empty;

	/// <summary>
	/// AI answer (for "Odpowiedź" column).
	/// </summary>
	public string Answer { get; init; } = string.Empty;

	/// <summary>
	/// Iteration count (for "Liczba iteracji" column).
	/// </summary>
	public int IterationCount { get; init; }
}
