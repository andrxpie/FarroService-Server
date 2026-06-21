# syntax=docker/dockerfile:1

# ============================================================
# Build stage — full .NET 10 SDK to restore + publish
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project files first so `dotnet restore` is cached
# independently of source changes. All three projects in the solution
# are needed because WebAPI -> BLL -> DAL via ProjectReference.
COPY ["FarroService.WebAPI/FarroService.WebAPI.csproj", "FarroService.WebAPI/"]
COPY ["FarroService.BLL/FarroService.BLL.csproj", "FarroService.BLL/"]
COPY ["FarroService.DAL/FarroService.DAL.csproj", "FarroService.DAL/"]
RUN dotnet restore "FarroService.WebAPI/FarroService.WebAPI.csproj"

# Copy the remaining source and publish a Release build.
COPY . .
RUN dotnet publish "FarroService.WebAPI/FarroService.WebAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ============================================================
# Runtime stage — slim ASP.NET 10 runtime only
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Production by default; PORT is the port Kestrel binds to (Program.cs reads it).
# Fly.io overrides PORT at runtime; 8080 is the local/default fallback.
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FarroService.WebAPI.dll"]
