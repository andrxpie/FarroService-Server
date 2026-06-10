using FarroService.BLL;
using FarroService.WebAPI.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVICES CONFIGURATION VIA EXTENSIONS
// ==========================================

// Єдина правильна CORS політика для Next.js
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://26.28.0.238:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Дозволяє передавати токени
    });
});

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddRepositoryWrapper();
builder.Services.AddIdentityServices();
builder.Services.AddExternalServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

// ЗВЕРНИ УВАГУ: Я видалив дублікат builder.Services.AddCors("CorsPolicy"...)

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BllAssemblyMarker).Assembly));
builder.Services.AddControllers();
builder.Services.AddOpenApiDocumentation();

var app = builder.Build();

// ==========================================
// 2. MIDDLEWARE PIPELINE (ПОРЯДОК МАЄ ЗНАЧЕННЯ!)
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Farro Service API Docs")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// 1. Спочатку маршрутизація
app.UseRouting();

// 2. ПОТІМ CORS (обов'язково між Routing та Auth)
app.UseCors("AllowNextJS");

// 3. Авторизація та аутентифікація
app.UseAuthentication();
app.UseAuthorization();

// 4. Мапінг контролерів
app.MapControllers();

// Uncomment if you want automatically apply migrations and seed initial data on application startup
// await app.SeedDatabaseAsync();

app.Run();