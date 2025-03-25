# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PersonalFinanceApplication.csproj", "./"]
RUN dotnet restore "./PersonalFinanceApplication.csproj"
COPY . .
RUN dotnet publish "./PersonalFinanceApplication.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Environment variables (customize these!)
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Expose the port your app uses (must match ASPNETCORE_URLS)
EXPOSE 8080

ENTRYPOINT ["dotnet", "PersonalFinanceApplication.dll"]