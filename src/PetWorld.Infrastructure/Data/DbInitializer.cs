using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Entities;

namespace PetWorld.Infrastructure.Data;

/// <summary>
/// Database initializer - seeds products on startup.
/// </summary>
public static class DbInitializer
{
	/// <summary>
	/// Seeds the database with 10 products from requirements.
	/// </summary>
	public static async Task SeedDataAsync(PetWorldDbContext context)
	{
		// Check if products already exist
		if (await context.Products.AnyAsync())
		{
			return; // Database already seeded
		}

		var products = new List<Product>
		{
			new()
			{
				Name = "Royal Canin Adult Dog 15kg",
				Category = "Karma dla psów",
				Price = 289m,
				Description = "Premium karma dla doros³ych psów œrednich ras"
			},
			new()
			{
				Name = "Whiskas Adult Kurczak 7kg",
				Category = "Karma dla kotów",
				Price = 129m,
				Description = "Sucha karma dla doros³ych kotów z kurczakiem"
			},
			new()
			{
				Name = "Tetra AquaSafe 500ml",
				Category = "Akwarystyka",
				Price = 45m,
				Description = "Uzdatniacz wody do akwarium, neutralizuje chlor"
			},
			new()
			{
				Name = "Trixie Drapak XL 150cm",
				Category = "Akcesoria dla kotów",
				Price = 399m,
				Description = "Wysoki drapak z platformami i domkiem"
			},
			new()
			{
				Name = "Kong Classic Large",
				Category = "Zabawki dla psów",
				Price = 69m,
				Description = "Wytrzyma³a zabawka do nape³niania smako³ykami"
			},
			new()
			{
				Name = "Ferplast Klatka dla chomika",
				Category = "Gryzonie",
				Price = 189m,
				Description = "Klatka 60x40cm z wyposa¿eniem"
			},
			new()
			{
				Name = "Flexi Smycz automatyczna 8m",
				Category = "Akcesoria dla psów",
				Price = 119m,
				Description = "Smycz zwijana dla psów do 50kg"
			},
			new()
			{
				Name = "Brit Premium Kitten 8kg",
				Category = "Karma dla kotów",
				Price = 159m,
				Description = "Karma dla koci¹t do 12 miesi¹ca ¿ycia"
			},
			new()
			{
				Name = "JBL ProFlora CO2 Set",
				Category = "Akwarystyka",
				Price = 549m,
				Description = "Kompletny zestaw CO2 dla roœlin akwariowych"
			},
			new()
			{
				Name = "Vitapol Siano dla królików 1kg",
				Category = "Gryzonie",
				Price = 25m,
				Description = "Naturalne siano ³¹kowe, podstawa diety"
			}
		};

		context.Products.AddRange(products);
		context.SaveChanges();
	}
}
