using FarroService.BLL;
using FarroService.WebAPI.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. SERVICES CONFIGURATION VIA EXTENSIONS
// ==========================================

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddRepositoryWrapper();
builder.Services.AddIdentityServices();
builder.Services.AddExternalServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCors(opts => opts.AddPolicy("CorsPolicy", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BllAssemblyMarker).Assembly));

builder.Services.AddControllers();
builder.Services.AddOpenApiDocumentation();

var app = builder.Build();

// ==========================================
// 2. MIDDLEWARE PIPELINE
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

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Uncomment if you want automatically apply migrations and seed initial data on application startup
// await app.SeedDatabaseAsync();

app.Run();