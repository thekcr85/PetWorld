using PetWorld.Domain.Entities;

namespace PetWorld.Domain.Interfaces.Services;

/// <summary>
/// AI service implementing Writer-Critic pattern.
/// Writer generates response → Critic evaluates → Iterate up to 3 times.
/// This is the core business logic of the application.
/// </summary>
public interface IAiChatService
{
	/// <summary>
	/// Processes customer question using Writer-Critic workflow.
	/// 
	/// Workflow:
	/// 1. Writer generates initial answer based on products
	/// 2. Critic evaluates: approved=true → return, approved=false → feedback
	/// 3. If not approved: Writer improves based on feedback
	/// 4. Repeat max 3 iterations
	/// 5. Return final answer + iteration count
	/// </summary>
	/// <param name="question">Customer's question about pet products</param>
	/// <param name="products">Available products for AI to recommend</param>
	/// <returns>Tuple: (final answer, number of iterations performed)</returns>
	Task<(string Answer, int IterationCount)> GetAnswerAsync(
		string question,
		IEnumerable<Product> products);
}
