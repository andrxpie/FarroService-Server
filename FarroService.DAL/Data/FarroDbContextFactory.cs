using FarroService.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Reflection;

namespace FarroService.DAL.Data;

/// <summary>
/// A factory for generating database context during development (migrations).
/// </summary>
public class FarroDbContextFactory : IDesignTimeDbContextFactory<FarroServiceDbContext>
{
    public FarroServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FarroServiceDbContext>();

        var configBuilder = new ConfigurationBuilder();

        var assemblyWithSecrets = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(assembly => assembly.GetCustomAttribute<UserSecretsIdAttribute>() != null);

        configBuilder.AddUserSecrets(assemblyWithSecrets!, optional: true);

        var configuration = configBuilder.Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        return new FarroServiceDbContext(optionsBuilder.Options);
    }
}
