using Microsoft.EntityFrameworkCore;
using PetWorld.Application.Services;
using PetWorld.Domain.Interfaces.Repositories;
using PetWorld.Domain.Interfaces.Services;
using PetWorld.Infrastructure.Data;
using PetWorld.Infrastructure.Repositories;
using PetWorld.Infrastructure.Services;
using PetWorld.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<PetWorldDbContext>(options =>
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Dependency Injection - Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();

// Dependency Injection - Services
builder.Services.AddScoped<IAiChatService, AiChatService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

// Database initialization (seed products)
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<PetWorldDbContext>();
	
	// Retry logic for MySQL connection (Docker startup delay)
	var retries = 5;
	while (retries > 0)
	{
		try
		{
			context.Database.EnsureCreated();
			DbInitializer.SeedData(context);
			break;
		}
		catch
		{
			retries--;
			if (retries == 0) throw;
			Thread.Sleep(3000);
		}
	}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
