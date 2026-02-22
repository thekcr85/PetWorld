using Microsoft.EntityFrameworkCore;
using PetWorld.Application.Services;
using PetWorld.Domain.Interfaces.Repositories;
using PetWorld.Domain.Interfaces.Services;
using PetWorld.Infrastructure.Data;
using PetWorld.Infrastructure.Repositories;
using PetWorld.Infrastructure.Services;
using PetWorld.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<PetWorldDbContext>(options =>
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
builder.Services.AddScoped<IAiChatService, AiChatService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<PetWorldDbContext>();
	await context.Database.EnsureCreatedAsync();
	await DbInitializer.SeedDataAsync(context);
}

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
