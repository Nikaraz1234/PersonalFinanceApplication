# Use the official .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PersonalFinanceApplication.csproj", "./"]
RUN dotnet restore "./PersonalFinanceApplication.csproj"
COPY . .
RUN dotnet publish "./PersonalFinanceApplication.csproj" -c Release -o /app/publish

# Use the runtime image again to run the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PersonalFinanceApplication.dll"]
