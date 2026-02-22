using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.Interfaces.Repositories;
using PetWorld.Application.Interfaces.Services;
using PetWorld.Infrastructure.Data;
using PetWorld.Infrastructure.Repositories;
using PetWorld.Infrastructure.Services;

namespace PetWorld.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
	public static IServiceCollection AddInfrastructureServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		services.AddDbContext<PetWorldDbContext>(options =>
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

		services.AddScoped<IProductRepository, ProductRepository>();
		services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
		services.AddScoped<IAiChatService, AiChatService>();

		return services;
	}
}