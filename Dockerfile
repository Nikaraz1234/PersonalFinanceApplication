# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PersonalFinanceApplication.csproj", "./"]
RUN dotnet restore "./PersonalFinanceApplication.csproj"
COPY . .
RUN dotnet publish "./PersonalFinanceApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Critical environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080  # Forces port 8080 in all environments

# Health check (Render-compatible)
HEALTHCHECK --interval=30s --timeout=3s \
  CMD curl -f http://localhost:8080/ || exit 1  # Changed to root endpoint

EXPOSE 8080
ENTRYPOINT ["dotnet", "PersonalFinanceApplication.dll"]