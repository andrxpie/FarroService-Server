using FarroService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarroService.DAL.Persistence.Configurations;

/// <summary>
/// Database configuration for the Booking entity using Fluent API.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ClientName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.ClientPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Pending");

        builder.Property(b => b.Address)
            .HasMaxLength(250);

        builder.Property(b => b.Latitude)
            .HasMaxLength(50);

        builder.Property(b => b.Longitude)
            .HasMaxLength(50);

        builder.Property(b => b.Comment)
            .HasMaxLength(500);

        builder.HasOne(b => b.Service)
            .WithMany()
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Master)
            .WithMany()
            .HasForeignKey(b => b.MasterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
