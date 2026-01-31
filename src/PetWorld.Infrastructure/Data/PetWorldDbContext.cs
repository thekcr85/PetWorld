using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Entities;

namespace PetWorld.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for PetWorld application.
/// Manages Product and ChatHistory entities.
/// </summary>
public sealed class PetWorldDbContext(DbContextOptions<PetWorldDbContext> options) : DbContext(options)
{
	public DbSet<Product> Products => Set<Product>();
	public DbSet<ChatHistory> ChatHistories => Set<ChatHistory>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Product entity configuration
		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasKey(p => p.Id);
			entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
			entity.Property(p => p.Category).IsRequired().HasMaxLength(100);
			entity.Property(p => p.Price).HasPrecision(18, 2);
			entity.Property(p => p.Description).IsRequired().HasMaxLength(1000);
		});

		// ChatHistory entity configuration
		modelBuilder.Entity<ChatHistory>(entity =>
		{
			entity.HasKey(c => c.Id);
			entity.Property(c => c.Question).IsRequired().HasMaxLength(2000);
			entity.Property(c => c.Answer).IsRequired().HasMaxLength(5000);
			entity.Property(c => c.IterationCount).IsRequired();
			entity.Property(c => c.CreatedAt).IsRequired();
			entity.HasIndex(c => c.CreatedAt);
		});
	}
}
