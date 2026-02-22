using PetWorld.Domain.Entities;

namespace PetWorld.Domain.Interfaces.Repositories;

/// <summary>
/// Repository for accessing product catalog.
/// Used by AI service to get product information for recommendations.
/// </summary>
public interface IProductRepository
{
	/// <summary>
	/// Gets all products from the catalog.
	/// Used to pass product information to AI Writer agent.
	/// </summary>
	/// <returns>List of all products</returns>
	Task<IEnumerable<Product>> GetAllAsync();
}

