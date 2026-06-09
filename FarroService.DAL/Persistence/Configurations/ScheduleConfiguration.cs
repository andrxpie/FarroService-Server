using FarroService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarroService.DAL.Persistence.Configurations;

/// <summary>
/// Database configuration for the Schedule entity using Fluent API.
/// </summary>
public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder.HasKey(s => s.Id);

        // Setting up master cascade deletion (if master is deleted, working schedules are deleted too)
        builder.HasOne(s => s.Master)
            .WithMany()
            .HasForeignKey(s => s.MasterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
