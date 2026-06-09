using FarroService.DAL.Entities;
using FarroService.DAL.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FarroService.DAL.Data;

/// <summary>
/// Database schema for automatically adding roles, administrators, masters, and services.
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<FarroServiceDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        string[] roles = { "Admin", "Master", "Guest" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        var adminEmail = "admin@farro.ua";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Ушаков Микола Ігорович",
                EmailConfirmed = true
            };
            if ((await userManager.CreateAsync(admin, "Admin123!")).Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        var mastersData = new List<(string Email, string Name, string Spec)>
        {
            ("koval@farro.ua", "Ковальчук Дмитро Олександрович", "Монтаж опалювального обладнання"),
            ("shev@farro.ua", "Шевченко Андрій Петрович", "Встановлення преміум-сантехніки")
        };

        var masterIds = new List<Guid>();
        foreach (var mData in mastersData)
        {
            var master = await userManager.FindByEmailAsync(mData.Email);
            if (master == null)
            {
                master = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = mData.Email,
                    Email = mData.Email,
                    FullName = mData.Name,
                    MasterSpecialization = mData.Spec,
                    EmailConfirmed = true
                };
                if ((await userManager.CreateAsync(master, "Master123!")).Succeeded)
                    await userManager.AddToRoleAsync(master, "Master");
            }
            masterIds.Add(master.Id);
        }

        if (!context.Services.Any())
        {
            context.Services.AddRange(
                new Service { Id = Guid.NewGuid(), Title = "Професійна інженерна консультація", Description = "Консультація з підбору сантехніки та опалення", DurationMinutes = 60, Price = 500 },
                new Service { Id = Guid.NewGuid(), Title = "Монтаж та підключення бойлера", Description = "Встановлення водонагрівача з гарантією", DurationMinutes = 120, Price = 1800 },
                new Service { Id = Guid.NewGuid(), Title = "Монтаж інсталяції та унітазу", Description = "Прихований монтаж санфаянсу", DurationMinutes = 180, Price = 2500 }
            );
            await context.SaveChangesAsync();
        }

        if (!context.Schedules.Any() && masterIds.Any())
        {
            var days = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            foreach (var masterId in masterIds)
            {
                foreach (var day in days)
                {
                    context.Schedules.Add(new Schedule
                    {
                        Id = Guid.NewGuid(),
                        MasterId = masterId,
                        DayOfWeek = day,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(18, 0, 0),
                        IsWorkingDay = true
                    });
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
