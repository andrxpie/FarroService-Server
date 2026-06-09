using FarroService.DAL.Data;
using FarroService.DAL.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FarroService.WebAPI.Extensions;

/// <summary>
/// Extension methods for WebApplication to handle database startup tasks.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Automatically applies pending migrations and seeds the database with initial lookup data on application startup.
    /// </summary>
    /// <param name="app">The running web application instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Database initialization starting: checking for pending migrations...");

            var context = services.GetRequiredService<FarroServiceDbContext>();

            // Automatically apply any pending EF Core migrations
            await context.Database.MigrateAsync();

            logger.LogInformation("Database migration applied successfully. Seeding initial data...");

            // Run the main seeder to provision roles, default admin, masters, schedules, and plumbing services
            await DatabaseSeeder.SeedAsync(services);

            logger.LogInformation("✓ Database migration and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "A critical error occurred while migrating or seeding the database on startup.");
            throw;
        }
    }
}
