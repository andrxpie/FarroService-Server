using FarroService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarroService.DAL.Persistence.Configurations;

/// <summary>
/// Database configuration for the custom ApplicationUser entity using Fluent API.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.MasterSpecialization)
            .HasMaxLength(250);

        builder.Property(u => u.CreatedAt)
            .IsRequired();
    }
}
