using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.ConfigureEndpointDefaults(listenOptions => {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

builder.WebHost.UseUrls("http://*:8080");
// Add services to the container
var connectionString = builder.Configuration.GetConnectionString("SupabaseConnection");

// Supabase PostgreSQL connection with better error handling
builder.Services.AddScoped<NpgsqlConnection>(_ =>
{
    var connection = new NpgsqlConnection(connectionString);
    connection.Open(); // Will throw if fails
    Console.WriteLine("Connected to Supabase PostgreSQL");
    return connection;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Forward headers for Render's proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run(); // Let Docker/Render handle the port