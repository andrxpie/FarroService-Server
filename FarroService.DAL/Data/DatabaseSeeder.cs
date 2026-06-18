using FarroService.DAL.Entities;
using FarroService.DAL.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FarroService.DAL.Data;

public static class DatabaseSeeder
{
    // Deterministic Ids keep the seeder idempotent — re-running adds only missing rows.
    private static Guid SpecId(int n) => new($"11111111-0000-0000-0000-{n:D12}");
    private static Guid ServiceId(int n) => new($"22222222-0000-0000-0000-{n:D12}");

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

        // 2. Specializations (add-if-missing by Id)
        var specializations = new[]
        {
            new Specialization { Id = SpecId(1),  Name = "Сантехніка" },
            new Specialization { Id = SpecId(2),  Name = "Опалення" },
            new Specialization { Id = SpecId(3),  Name = "Каналізація" },
            new Specialization { Id = SpecId(4),  Name = "Водопостачання" },
            new Specialization { Id = SpecId(5),  Name = "Газове обладнання" },
            new Specialization { Id = SpecId(6),  Name = "Бойлери та водонагрівачі" },
            new Specialization { Id = SpecId(7),  Name = "Фільтрація води" },
            new Specialization { Id = SpecId(8),  Name = "Вентиляція та кондиціонування" },
            new Specialization { Id = SpecId(9),  Name = "Аварійна служба" },
            new Specialization { Id = SpecId(10), Name = "Тепла підлога" },
            new Specialization { Id = SpecId(11), Name = "Насосне обладнання" },
            new Specialization { Id = SpecId(12), Name = "Монтаж під ключ" }
        };
        var existingSpecIds = await context.Specializations.Select(s => s.Id).ToListAsync();
        var specsToAdd = specializations.Where(s => !existingSpecIds.Contains(s.Id)).ToList();
        if (specsToAdd.Count > 0)
        {
            context.Specializations.AddRange(specsToAdd);
            await context.SaveChangesAsync();
        }

        // 3. Services (add-if-missing by Id)
        var services = new[]
        {
            // Сантехніка
            new Service { Id = ServiceId(1),  Title = "Ремонт крана",                       Description = "Заміна або ремонт змішувача будь-якого типу",            DurationMinutes = 60,  Price = 500m,   SpecializationId = SpecId(1) },
            new Service { Id = ServiceId(2),  Title = "Встановлення раковини",              Description = "Монтаж навісної або підлогової раковини",                DurationMinutes = 90,  Price = 900m,   SpecializationId = SpecId(1) },
            new Service { Id = ServiceId(3),  Title = "Інженерна консультація",             Description = "Підбір обладнання та проектування системи",              DurationMinutes = 60,  Price = 500m,   SpecializationId = SpecId(1) },
            new Service { Id = ServiceId(4),  Title = "Встановлення унітазу",               Description = "Монтаж та підключення підвісного або підлогового унітазу", DurationMinutes = 90,  Price = 1100m,  SpecializationId = SpecId(1) },
            new Service { Id = ServiceId(5),  Title = "Встановлення душової кабіни",        Description = "Збірка та підключення душової кабіни під ключ",          DurationMinutes = 180, Price = 2800m,  SpecializationId = SpecId(1) },

            // Опалення
            new Service { Id = ServiceId(6),  Title = "Встановлення котла",                 Description = "Монтаж та підключення газового або електрокотла",        DurationMinutes = 120, Price = 2000m,  SpecializationId = SpecId(2) },
            new Service { Id = ServiceId(7),  Title = "Налаштування радіатора",            Description = "Балансування та промивання системи опалення",            DurationMinutes = 90,  Price = 1200m,  SpecializationId = SpecId(2) },
            new Service { Id = ServiceId(8),  Title = "Заміна радіатора опалення",         Description = "Демонтаж старого та встановлення нового радіатора",      DurationMinutes = 120, Price = 1800m,  SpecializationId = SpecId(2) },
            new Service { Id = ServiceId(9),  Title = "Промивання системи опалення",       Description = "Гідрохімічне промивання труб і радіаторів",              DurationMinutes = 150, Price = 2200m,  SpecializationId = SpecId(2) },
            new Service { Id = ServiceId(10), Title = "Діагностика системи опалення",      Description = "Виявлення несправностей та оцінка стану системи",        DurationMinutes = 60,  Price = 600m,   SpecializationId = SpecId(2) },

            // Каналізація
            new Service { Id = ServiceId(11), Title = "Прочищення каналізації",            Description = "Механічне та гідродинамічне прочищення труб",            DurationMinutes = 60,  Price = 700m,   SpecializationId = SpecId(3) },
            new Service { Id = ServiceId(12), Title = "Заміна каналізаційних труб",        Description = "Демонтаж та монтаж внутрішньої каналізації",             DurationMinutes = 180, Price = 2300m,  SpecializationId = SpecId(3) },
            new Service { Id = ServiceId(13), Title = "Встановлення септика",              Description = "Монтаж автономної каналізаційної системи",               DurationMinutes = 240, Price = 5000m,  SpecializationId = SpecId(3) },
            new Service { Id = ServiceId(14), Title = "Усунення засору гідродинамікою",    Description = "Прочищення складних засорів водою під тиском",           DurationMinutes = 90,  Price = 1400m,  SpecializationId = SpecId(3) },

            // Водопостачання
            new Service { Id = ServiceId(15), Title = "Заміна труб водопостачання",        Description = "Повна заміна внутрішнього водопроводу",                  DurationMinutes = 180, Price = 2500m,  SpecializationId = SpecId(4) },
            new Service { Id = ServiceId(16), Title = "Встановлення лічильника води",      Description = "Монтаж та опломбування лічильника холодної/гарячої води", DurationMinutes = 60,  Price = 800m,   SpecializationId = SpecId(4) },
            new Service { Id = ServiceId(17), Title = "Розведення водопроводу",            Description = "Проектування та монтаж водопроводу в новому приміщенні", DurationMinutes = 240, Price = 3500m,  SpecializationId = SpecId(4) },
            new Service { Id = ServiceId(18), Title = "Усунення протікання труби",         Description = "Локалізація та ремонт місця протікання",                 DurationMinutes = 60,  Price = 700m,   SpecializationId = SpecId(4) },

            // Газове обладнання
            new Service { Id = ServiceId(19), Title = "Підключення газової плити",          Description = "Монтаж та підключення газової плити з перевіркою герметичності", DurationMinutes = 90,  Price = 1300m,  SpecializationId = SpecId(5) },
            new Service { Id = ServiceId(20), Title = "Встановлення газової колонки",       Description = "Монтаж та підключення проточного водонагрівача",         DurationMinutes = 150, Price = 2400m,  SpecializationId = SpecId(5) },
            new Service { Id = ServiceId(21), Title = "Діагностика газового обладнання",    Description = "Перевірка тяги, герметичності та справності приладів",   DurationMinutes = 60,  Price = 700m,   SpecializationId = SpecId(5) },
            new Service { Id = ServiceId(22), Title = "Заміна газового шлангу",             Description = "Заміна підвідного шлангу та з'єднань",                   DurationMinutes = 45,  Price = 500m,   SpecializationId = SpecId(5) },

            // Бойлери та водонагрівачі
            new Service { Id = ServiceId(23), Title = "Встановлення бойлера",               Description = "Монтаж та підключення накопичувального водонагрівача",   DurationMinutes = 120, Price = 1600m,  SpecializationId = SpecId(6) },
            new Service { Id = ServiceId(24), Title = "Чищення бойлера від накипу",         Description = "Розбирання, чищення баку та заміна анода",               DurationMinutes = 90,  Price = 1000m,  SpecializationId = SpecId(6) },
            new Service { Id = ServiceId(25), Title = "Заміна ТЕНа бойлера",                Description = "Демонтаж та встановлення нового нагрівального елемента",  DurationMinutes = 60,  Price = 800m,   SpecializationId = SpecId(6) },
            new Service { Id = ServiceId(26), Title = "Ремонт водонагрівача",               Description = "Діагностика та усунення несправностей бойлера",          DurationMinutes = 90,  Price = 1100m,  SpecializationId = SpecId(6) },

            // Фільтрація води
            new Service { Id = ServiceId(27), Title = "Встановлення фільтра для води",      Description = "Монтаж магістрального або проточного фільтра",           DurationMinutes = 90,  Price = 1200m,  SpecializationId = SpecId(7) },
            new Service { Id = ServiceId(28), Title = "Заміна картриджів фільтра",          Description = "Планова заміна фільтруючих елементів",                   DurationMinutes = 30,  Price = 400m,   SpecializationId = SpecId(7) },
            new Service { Id = ServiceId(29), Title = "Монтаж системи зворотного осмосу",   Description = "Встановлення багатоступеневої системи очищення води",    DurationMinutes = 120, Price = 2600m,  SpecializationId = SpecId(7) },
            new Service { Id = ServiceId(30), Title = "Встановлення пом'якшувача води",     Description = "Монтаж системи пом'якшення та знезалізнення води",       DurationMinutes = 150, Price = 3000m,  SpecializationId = SpecId(7) },

            // Вентиляція та кондиціонування
            new Service { Id = ServiceId(31), Title = "Встановлення кондиціонера",          Description = "Монтаж спліт-системи з прокладанням траси",              DurationMinutes = 180, Price = 3500m,  SpecializationId = SpecId(8) },
            new Service { Id = ServiceId(32), Title = "Чищення кондиціонера",               Description = "Чищення фільтрів, випарника та дренажу",                 DurationMinutes = 60,  Price = 700m,   SpecializationId = SpecId(8) },
            new Service { Id = ServiceId(33), Title = "Заправка кондиціонера фреоном",      Description = "Перевірка системи та дозаправка холодоагентом",          DurationMinutes = 60,  Price = 900m,   SpecializationId = SpecId(8) },
            new Service { Id = ServiceId(34), Title = "Монтаж припливної вентиляції",       Description = "Встановлення системи припливно-витяжної вентиляції",     DurationMinutes = 240, Price = 4500m,  SpecializationId = SpecId(8) },

            // Аварійна служба
            new Service { Id = ServiceId(35), Title = "Аварійний виклик сантехніка",        Description = "Терміновий виїзд майстра протягом години",               DurationMinutes = 60,  Price = 1000m,  SpecializationId = SpecId(9) },
            new Service { Id = ServiceId(36), Title = "Усунення прориву труби",             Description = "Терміновий ремонт прориву водопроводу",                  DurationMinutes = 90,  Price = 1500m,  SpecializationId = SpecId(9) },
            new Service { Id = ServiceId(37), Title = "Аварійне відкачування води",         Description = "Відкачування води при затопленні приміщення",            DurationMinutes = 120, Price = 1800m,  SpecializationId = SpecId(9) },
            new Service { Id = ServiceId(38), Title = "Розморожування труб",                Description = "Відігрівання замерзлих ділянок трубопроводу",             DurationMinutes = 90,  Price = 1300m,  SpecializationId = SpecId(9) },

            // Тепла підлога
            new Service { Id = ServiceId(39), Title = "Монтаж водяної теплої підлоги",      Description = "Укладання контурів та підключення до системи опалення",  DurationMinutes = 300, Price = 6000m,  SpecializationId = SpecId(10) },
            new Service { Id = ServiceId(40), Title = "Монтаж електричної теплої підлоги",  Description = "Укладання нагрівального кабелю або матів",               DurationMinutes = 240, Price = 4800m,  SpecializationId = SpecId(10) },
            new Service { Id = ServiceId(41), Title = "Підключення терморегулятора",        Description = "Встановлення та налаштування терморегулятора",           DurationMinutes = 60,  Price = 700m,   SpecializationId = SpecId(10) },
            new Service { Id = ServiceId(42), Title = "Діагностика теплої підлоги",         Description = "Пошук несправностей та перевірка контурів",              DurationMinutes = 90,  Price = 900m,   SpecializationId = SpecId(10) },

            // Насосне обладнання
            new Service { Id = ServiceId(43), Title = "Встановлення насосної станції",      Description = "Монтаж та налаштування станції водопостачання",          DurationMinutes = 150, Price = 2800m,  SpecializationId = SpecId(11) },
            new Service { Id = ServiceId(44), Title = "Ремонт циркуляційного насоса",       Description = "Діагностика та ремонт насоса системи опалення",          DurationMinutes = 90,  Price = 1200m,  SpecializationId = SpecId(11) },
            new Service { Id = ServiceId(45), Title = "Встановлення дренажного насоса",      Description = "Монтаж насоса для відведення стічних вод",               DurationMinutes = 120, Price = 1700m,  SpecializationId = SpecId(11) },
            new Service { Id = ServiceId(46), Title = "Обслуговування свердловинного насоса", Description = "Перевірка, чищення та налаштування глибинного насоса",  DurationMinutes = 120, Price = 2000m,  SpecializationId = SpecId(11) },

            // Монтаж під ключ
            new Service { Id = ServiceId(47), Title = "Сантехніка під ключ (новобудова)",   Description = "Повний монтаж усіх сантехнічних систем у новій квартирі", DurationMinutes = 480, Price = 12000m, SpecializationId = SpecId(12) },
            new Service { Id = ServiceId(48), Title = "Розведення труб у санвузлі",         Description = "Монтаж водопостачання та каналізації в санвузлі",        DurationMinutes = 300, Price = 5500m,  SpecializationId = SpecId(12) },
            new Service { Id = ServiceId(49), Title = "Комплексний монтаж ванної кімнати",  Description = "Повне сантехнічне облаштування ванної кімнати",          DurationMinutes = 480, Price = 15000m, SpecializationId = SpecId(12) },
            new Service { Id = ServiceId(50), Title = "Заміна стояків у квартирі",          Description = "Заміна загальнобудинкових стояків водо- та каналізації", DurationMinutes = 360, Price = 7000m,  SpecializationId = SpecId(12) }
        };
        var existingServiceIds = await context.Services.Select(s => s.Id).ToListAsync();
        var servicesToAdd = services.Where(s => !existingServiceIds.Contains(s.Id)).ToList();
        if (servicesToAdd.Count > 0)
        {
            context.Services.AddRange(servicesToAdd);
            await context.SaveChangesAsync();
        }

        // 3b. Reconcile legacy services from earlier seeds (random Ids, duplicate titles)
        // onto the canonical deterministic set: repoint their bookings, then remove them.
        var canonicalIdByTitle = services.ToDictionary(s => s.Title, s => s.Id);
        var deterministicIds = services.Select(s => s.Id).ToHashSet();
        var legacyServices = await context.Services
            .Where(s => !deterministicIds.Contains(s.Id))
            .ToListAsync();
        var removedLegacy = false;
        foreach (var legacy in legacyServices)
        {
            // Leave genuinely custom services (titles outside the seeded set) untouched.
            if (!canonicalIdByTitle.TryGetValue(legacy.Title, out var canonicalId))
                continue;

            var referencingBookings = await context.Bookings
                .Where(b => b.ServiceId == legacy.Id)
                .ToListAsync();
            foreach (var booking in referencingBookings)
                booking.ServiceId = canonicalId;

            context.Services.Remove(legacy);
            removedLegacy = true;
        }
        if (removedLegacy)
            await context.SaveChangesAsync();

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
            (Email: "koval@farro.ua",      Name: "Ковальчук Дмитро Олександрович", Specs: new[] { SpecId(1), SpecId(2) }),
            (Email: "shev@farro.ua",       Name: "Шевченко Андрій Петрович",        Specs: new[] { SpecId(3), SpecId(4) }),
            (Email: "petro@farro.ua",      Name: "Петренко Марія Василівна",        Specs: new[] { SpecId(1), SpecId(3) }),
            (Email: "bond@farro.ua",       Name: "Бондаренко Олег Ігорович",        Specs: new[] { SpecId(5), SpecId(2) }),
            (Email: "tkach@farro.ua",      Name: "Ткаченко Сергій Миколайович",     Specs: new[] { SpecId(6), SpecId(4) }),
            (Email: "melnyk@farro.ua",     Name: "Мельник Ірина Степанівна",        Specs: new[] { SpecId(7), SpecId(1) }),
            (Email: "kravch@farro.ua",     Name: "Кравченко Віктор Анатолійович",   Specs: new[] { SpecId(8) }),
            (Email: "oliynyk@farro.ua",    Name: "Олійник Тарас Богданович",        Specs: new[] { SpecId(9), SpecId(3) }),
            (Email: "lysenko@farro.ua",    Name: "Лисенко Наталія Володимирівна",   Specs: new[] { SpecId(10), SpecId(2) }),
            (Email: "moroz@farro.ua",      Name: "Мороз Євген Павлович",            Specs: new[] { SpecId(11), SpecId(4) }),
            (Email: "sydorenko@farro.ua",  Name: "Сидоренко Роман Юрійович",        Specs: new[] { SpecId(12), SpecId(1) }),
            (Email: "marchenko@farro.ua",  Name: "Марченко Аліна Денисівна",        Specs: new[] { SpecId(5), SpecId(6) }),
            (Email: "savchenko@farro.ua",  Name: "Савченко Микола Іванович",        Specs: new[] { SpecId(2), SpecId(10) }),
            (Email: "polishchuk@farro.ua", Name: "Поліщук Юлія Андріївна",          Specs: new[] { SpecId(7), SpecId(4) }),
            (Email: "rudenko@farro.ua",    Name: "Руденко Артем Васильович",        Specs: new[] { SpecId(9), SpecId(1), SpecId(3) }),
            (Email: "tarasenko@farro.ua",  Name: "Тарасенко Оксана Леонідівна",     Specs: new[] { SpecId(8), SpecId(6) }),
            (Email: "boyko@farro.ua",      Name: "Бойко Владислав Олегович",        Specs: new[] { SpecId(11), SpecId(3) }),
            (Email: "didenko@farro.ua",    Name: "Діденко Світлана Миколаївна",     Specs: new[] { SpecId(12), SpecId(2) }),
            (Email: "zhuk@farro.ua",       Name: "Жук Максим Сергійович",           Specs: new[] { SpecId(1), SpecId(4), SpecId(5) })
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

        // 7. Schedules (Mon–Fri, 10:00–19:00) — added per master that has none yet
        var days = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        var addedSchedule = false;
        foreach (var masterId in masterIds)
        {
            if (await context.Schedules.AnyAsync(s => s.MasterId == masterId))
                continue;

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
            addedSchedule = true;
        }
        if (addedSchedule)
            await context.SaveChangesAsync();
    }
}
