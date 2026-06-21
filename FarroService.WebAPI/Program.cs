using FarroService.BLL;
using FarroService.WebAPI.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Fly.io (and most container hosts) inject the listening port via the PORT env var.
// Bind Kestrel to 0.0.0.0:$PORT so the app is reachable inside the Fly machine; fall back to 8080 locally.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ==========================================
// 1. SERVICES CONFIGURATION VIA EXTENSIONS
// ==========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://26.28.0.238:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddRepositoryWrapper();
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BllAssemblyMarker).Assembly));
builder.Services.AddControllers();
builder.Services.AddOpenApiDocumentation();

var app = builder.Build();

// ==========================================
// 2. MIDDLEWARE PIPELINE
// ==========================================

// API docs (Scalar + OpenAPI) are exposed in all environments so the deployed API can be demoed.
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Farro Service API Docs")
           .WithTheme(ScalarTheme.DeepSpace)
           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseRouting();
app.UseCors("AllowNextJS");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Lightweight public health endpoint for Fly.io health checks.
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// Apply pending EF Core migrations and seed lookup/admin data on startup (idempotent).
await app.SeedDatabaseAsync();

app.Run();