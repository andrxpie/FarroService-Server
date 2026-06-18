using FarroService.DAL.Entities;
using FarroService.DAL.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FarroService.DAL.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<FarroServiceDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // 1. Roles
        string[] roles = { "Master", "Admin", "MainAdmin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        // 2. Specializations
        if (!await context.Specializations.AnyAsync())
        {
            context.Specializations.AddRange(
                new Specialization { Id = new Guid("11111111-0000-0000-0000-000000000001"), Name = "Сантехніка" },
                new Specialization { Id = new Guid("11111111-0000-0000-0000-000000000002"), Name = "Опалення" },
                new Specialization { Id = new Guid("11111111-0000-0000-0000-000000000003"), Name = "Каналізація" },
                new Specialization { Id = new Guid("11111111-0000-0000-0000-000000000004"), Name = "Водопостачання" }
            );
            await context.SaveChangesAsync();
        }

        var specPlumbing  = await context.Specializations.FirstAsync(s => s.Id == new Guid("11111111-0000-0000-0000-000000000001"));
        var specHeating   = await context.Specializations.FirstAsync(s => s.Id == new Guid("11111111-0000-0000-0000-000000000002"));
        var specSewage    = await context.Specializations.FirstAsync(s => s.Id == new Guid("11111111-0000-0000-0000-000000000003"));
        var specWater     = await context.Specializations.FirstAsync(s => s.Id == new Guid("11111111-0000-0000-0000-000000000004"));

        // 3. Services
        if (!await context.Services.AnyAsync())
        {
            context.Services.AddRange(
                new Service { Id = Guid.NewGuid(), Title = "Ремонт крана",                       Description = "Заміна або ремонт змішувача будь-якого типу",     DurationMinutes = 60,  Price = 500m,  SpecializationId = specPlumbing.Id },
                new Service { Id = Guid.NewGuid(), Title = "Встановлення раковини",              Description = "Монтаж навісної або підлогової раковини",         DurationMinutes = 90,  Price = 900m,  SpecializationId = specPlumbing.Id },
                new Service { Id = Guid.NewGuid(), Title = "Інженерна консультація",             Description = "Підбір обладнання та проектування системи",       DurationMinutes = 60,  Price = 500m,  SpecializationId = specPlumbing.Id },
                new Service { Id = Guid.NewGuid(), Title = "Встановлення котла",                 Description = "Монтаж та підключення газового або електрокотла", DurationMinutes = 120, Price = 2000m, SpecializationId = specHeating.Id },
                new Service { Id = Guid.NewGuid(), Title = "Налаштування радіатора",             Description = "Балансування та промивання системи опалення",     DurationMinutes = 90,  Price = 1200m, SpecializationId = specHeating.Id },
                new Service { Id = Guid.NewGuid(), Title = "Прочищення каналізації",             Description = "Механічне та гідродинамічне прочищення труб",     DurationMinutes = 60,  Price = 700m,  SpecializationId = specSewage.Id },
                new Service { Id = Guid.NewGuid(), Title = "Заміна труб водопостачання",         Description = "Повна заміна внутрішнього водопроводу",           DurationMinutes = 180, Price = 2500m, SpecializationId = specWater.Id }
            );
            await context.SaveChangesAsync();
        }

        // 4. MainAdmin
        var mainAdminEmail = "admin@farro.ua";
        if (await userManager.FindByEmailAsync(mainAdminEmail) == null)
        {
            var mainAdmin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = mainAdminEmail,
                Email = mainAdminEmail,
                FullName = "Головний Адміністратор",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            if ((await userManager.CreateAsync(mainAdmin, "Admin123!")).Succeeded)
                await userManager.AddToRoleAsync(mainAdmin, "MainAdmin");
        }

        // 5. Admin
        var adminEmail = "manager@farro.ua";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Менеджер Сервісу",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            if ((await userManager.CreateAsync(admin, "Admin123!")).Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // 6. Masters with specializations
        var mastersData = new[]
        {
            (Email: "koval@farro.ua",  Name: "Ковальчук Дмитро Олександрович", Specs: new[] { specPlumbing.Id, specHeating.Id }),
            (Email: "shev@farro.ua",   Name: "Шевченко Андрій Петрович",        Specs: new[] { specSewage.Id, specWater.Id }),
            (Email: "petro@farro.ua",  Name: "Петренко Марія Василівна",         Specs: new[] { specPlumbing.Id, specSewage.Id })
        };

        var masterIds = new List<Guid>();
        foreach (var m in mastersData)
        {
            var existing = await userManager.FindByEmailAsync(m.Email);
            if (existing == null)
            {
                var master = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = m.Email,
                    Email = m.Email,
                    FullName = m.Name,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };
                if ((await userManager.CreateAsync(master, "Master123!")).Succeeded)
                {
                    await userManager.AddToRoleAsync(master, "Master");
                    existing = master;
                }
            }

            if (existing != null)
            {
                masterIds.Add(existing.Id);

                // Attach specializations via tracked DbContext
                var trackedMaster = await context.Users
                    .Include(u => u.Specializations)
                    .FirstOrDefaultAsync(u => u.Id == existing.Id);

                if (trackedMaster != null && !trackedMaster.Specializations.Any())
                {
                    var specs = await context.Specializations
                        .Where(s => m.Specs.Contains(s.Id))
                        .ToListAsync();
                    foreach (var spec in specs)
                        trackedMaster.Specializations.Add(spec);
                    await context.SaveChangesAsync();
                }
            }
        }

        // 7. Schedules (Mon–Fri, 10:00–19:00)
        if (!await context.Schedules.AnyAsync() && masterIds.Any())
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
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(19, 0, 0),
                        IsWorkingDay = true
                    });
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
