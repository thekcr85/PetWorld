using PetWorld.Infrastructure.Data;

namespace PetWorld.Web.Extensions;

public static class DatabaseExtensions
{
	public static async Task InitialiseDatabaseAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();

		var context = scope.ServiceProvider.GetRequiredService<PetWorldDbContext>();

		await context.Database.EnsureCreatedAsync();
		await DbInitializer.SeedDataAsync(context);
	}
}
