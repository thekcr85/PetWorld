namespace PetWorld.Application.DTOs;

/// <summary>
/// Request DTO for chat page.
/// Sent from Blazor form to ChatService.
/// </summary>
public sealed record ChatRequestDto
{
	/// <summary>
	/// Customer's question about pet products.
	/// Example: "What food is best for my senior cat?"
	/// </summary>
	public string Question { get; init; } = string.Empty;
}
