using FarroService.BLL;
using FarroService.WebAPI.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

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

app.UseRouting();
app.UseCors("AllowNextJS");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// await app.SeedDatabaseAsync();

app.Run();