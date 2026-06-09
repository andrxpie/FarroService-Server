# FarroService Backend

Це серверна частина системи автоматизації сервісного обслуговування FarroService.
Проект реалізований на **ASP.NET Core (.NET 10)** з використанням архітектури **Clean/N-Tier**.

## 🛠 Архітектурний стек
- **Runtime:** .NET 10
- **Архітектура:** Headless, CQRS (MediatR)
- **База даних:** PostgreSQL (хостинг [Neon.tech](https://neon.tech/))
- **ORM:** Entity Framework Core
- **Автентифікація:** JWT (JSON Web Tokens), ASP.NET Core Identity
- **Документація:** OpenAPI (Microsoft.AspNetCore.OpenApi) + **Scalar UI**
- **Геокодування:** OpenStreetMap Nominatim API (Resilience pattern)

## 📂 Структура проекту

```text
FarroService/
├── FarroService.DAL/         # Data Access Layer
│   ├── Entities/             # Domain entities (ApplicationUser, Booking, Service, Schedule)
│   ├── Persistence/          # DbContext, Configurations, Migrations
│   └── Repositories/         # Repository pattern & Unit of Work
├── FarroService.BLL/         # Business Logic Layer
│   ├── Dto/                  # Data Transfer Objects
│   ├── MediatR/              # CQRS Commands & Queries (Auth, Booking, Service)
│   └── ExternalServices/     # Geocoding services
└── FarroService.WebAPI/      # Presentation Layer
    ├── Controllers/          # REST API endpoints
    ├── Extensions/           # ServiceCollectionExtensions for DI
    └── Program.cs            # App configuration
