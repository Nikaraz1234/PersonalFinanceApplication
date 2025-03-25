using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;
using Microsoft.Extensions.Logging;
using Supabase; // Added missing using

var builder = WebApplication.CreateBuilder(args);

// Kestrel Configuration
builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.ConfigureEndpointDefaults(listenOptions => {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

builder.WebHost.UseUrls("http://*:8080");
var connectionString = builder.Configuration.GetConnectionString("SupabaseConnection");

// Database Configuration
builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        builder.Configuration["SupabaseUrl"],
        builder.Configuration["SupabaseKey"],
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }
        )
);




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Middleware Pipeline
app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.MapControllers();

app.MapGet("/", () => "API is running");
app.MapGet("/healthz", () => Results.Ok("Healthy"));
app.MapGet("/test", async (NpgsqlDataSource db) => {
    return Results.Ok(await db.CreateCommand("SELECT 1").ExecuteScalarAsync());
}); 

app.Run();