# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PersonalFinanceApplication.csproj", "./"]
RUN dotnet restore "./PersonalFinanceApplication.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Environment variables (no inline comments!)
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_EnableDiagnostics=0

# Health check
HEALTHCHECK --interval=10s --timeout=2s --start-period=5s \
  CMD curl -f http://localhost:8080/healthz || exit 1

EXPOSE 8080
ENTRYPOINT ["dotnet", "PersonalFinanceApplication.dll"]