using Integration.BrasilApi.Fipe.Domain.Interfaces;
using Integration.BrasilApi.Fipe.Infrastructure.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configuração da Licença do QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddControllersWithViews();

// Injeção de Dependência da Camada de Service/Infra
builder.Services.AddScoped<IFipeService, FipeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Fipe}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
