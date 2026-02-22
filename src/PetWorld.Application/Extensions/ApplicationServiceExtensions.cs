using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.Interfaces.Services;
using PetWorld.Application.Services;

namespace PetWorld.Application.Extensions;

public static class ApplicationServiceExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IChatService, ChatService>();

		return services;
	}
}