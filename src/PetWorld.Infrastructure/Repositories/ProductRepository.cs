using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Interfaces.Repositories;
using PetWorld.Infrastructure.Data;

namespace PetWorld.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of product repository.
/// </summary>
public sealed class ProductRepository(PetWorldDbContext context) : IProductRepository
{
	public async Task<IEnumerable<Product>> GetAllAsync()
	{
		return await context.Products
			.AsNoTracking()
			.ToListAsync();
	}
}
