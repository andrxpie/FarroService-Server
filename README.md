<div align="center">

# FarroService — Backend

A clean-architecture REST API for booking plumbing & home-maintenance services.

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Neon-336791?logo=postgresql&logoColor=white)](https://neon.tech/)
[![MediatR](https://img.shields.io/badge/MediatR-CQRS-orange)](https://github.com/jbogard/MediatR)
[![JWT](https://img.shields.io/badge/Auth-JWT%20Bearer-black?logo=jsonwebtokens)](https://jwt.io/)
[![Scalar](https://img.shields.io/badge/API%20Docs-Scalar-1A1A2E?logo=openapiinitiative&logoColor=white)](https://scalar.com/)

</div>

---

## Overview

FarroService Backend is the API powering a plumbing and home-services booking platform. It handles service catalog management, master (technician) scheduling, client bookings, and role-based administration — built with **Clean Architecture** principles on top of **ASP.NET Core**.

Clients book services through a simple public form (no account required), while admins and masters manage services, schedules, and bookings through authenticated endpoints.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | [ASP.NET Core 10](https://learn.microsoft.com/en-us/aspnet/core/) |
| Language | [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) |
| ORM | [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) |
| Application logic | [MediatR](https://github.com/jbogard/MediatR) (CQRS pattern) |
| Authentication | [ASP.NET Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity) + [JWT Bearer](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn) |
| Database | [PostgreSQL](https://www.postgresql.org/) hosted on [Neon](https://neon.tech/) |
| Geocoding | [Nominatim API](https://nominatim.org/release-docs/latest/api/Overview/) |
| API docs | [Scalar](https://scalar.com/) (OpenAPI UI) |

## Architecture

The solution follows a layered Clean Architecture approach:

```
backend/
├── FarroService.DAL/      # Entities, repositories, EF Core, migrations
├── FarroService.BLL/      # DTOs, MediatR commands/queries/handlers
└── FarroService.WebAPI/   # Controllers, DI configuration, Program.cs
```

- **DAL** — persistence layer: entity models, `DbContext`, repositories, and database migrations.
- **BLL** — business logic layer: request/response DTOs and CQRS handlers via MediatR.
- **WebAPI** — presentation layer: REST controllers, middleware, and application startup.

## Domain Model

| Entity | Description |
|---|---|
| `ApplicationUser` | Identity-based user (Admin / Master), extended with full name and specializations |
| `Service` | A bookable service with price, duration, and an associated specialization |
| `Specialization` | A category of expertise linking masters to the services they can perform |
| `Booking` | A client reservation for a service with a specific master, date, and time slot |
| `Schedule` | A master's weekly working hours per day of the week |

## Roles

| Role | Description |
|---|---|
| `MainAdmin` | Full system access |
| `Admin` | Manages services, masters, and bookings |
| `Master` | Manages own schedule and assigned bookings |

Clients do **not** have accounts — bookings are created via a public form using just a name and phone number.

## Business Rules

- Bookings are only allowed within working hours: **10:00–19:00**.
- A master can only be booked for a service that matches one of their specializations.
- Available time slots are calculated per master, per service, per day.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)
- A [PostgreSQL](https://www.postgresql.org/) database (e.g. a free [Neon](https://neon.tech/) instance)

### Setup

```bash
# Restore dependencies
dotnet restore

# Configure your database connection string via User Secrets
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your-connection-string>"

# Apply EF Core migrations
dotnet ef database update --project FarroService.DAL

# Run the API
dotnet run --project FarroService.WebAPI
```

> **Note:** Use the direct Neon connection endpoint (without `-pooler`) for migrations — the pooled endpoint can cache stale "database not found" errors after `dotnet ef database drop`.

### Seed Data

On first run, the database is seeded with:

- 4 specializations: Plumbing, Heating, Sewerage, Water Supply
- 7 services linked to those specializations
- A `MainAdmin` and an `Admin` account
- 3 masters, each with 2 specializations and a Mon–Fri, 10:00–19:00 schedule

| Role | Email | Password |
|---|---|---|
| MainAdmin | `admin@farro.ua` | `Admin123!` |
| Admin | `manager@farro.ua` | `Admin123!` |

## API Documentation

Full interactive API documentation is available via **Scalar** once the project is running:

```
https://localhost:<port>/scalar
```

It covers authentication, services, masters, specializations, schedules, bookings, and user administration endpoints, including required roles and request/response schemas, generated from the app's OpenAPI specification.

## Key Implementation Notes

- `options.MapInboundClaims = true` is required for JWT — in .NET 10 this defaults to `false`, which silently breaks role-based authorization.
- `AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)` is enabled to support `DateTime.Kind = Unspecified` values in queries against PostgreSQL.

---

<div align="center">

Part of the **FarroService** diploma project — see the [frontend repository](https://github.com/andrxpie/FarroService-Client) for the client application.

</div>
