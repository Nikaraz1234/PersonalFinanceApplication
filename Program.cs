using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using Npgsql;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Repositories;
using PersonalFinanceApplication.Services;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// ===== Configuration =====
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// ===== Kestrel Configuration =====
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

// Use port 8080 (for Docker/Cloud compatibility)
builder.WebHost.UseUrls("http://*:8080");

// ===== Services Configuration =====

// Supabase Client (PostgreSQL)
var supabaseUrl = builder.Configuration["Supabase:Url"] ?? throw new ArgumentNullException("Supabase:Url");
var supabaseKey = builder.Configuration["Supabase:Key"] ?? throw new ArgumentNullException("Supabase:Key");

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        supabaseUrl,
        supabaseKey,
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
            
        }
    )
);
// Add this to your services
builder.Services.AddSingleton<NpgsqlDataSource>(_ =>
    NpgsqlDataSource.Create(builder.Configuration.GetConnectionString("SupabaseConnection")));

builder.Services.AddScoped<BudgetRepository>();
builder.Services.AddScoped<TransactionRepository>();

builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// ===== Middleware Pipeline =====

// Security headers (recommended)
app.UseForwardedHeaders();

// Swagger (only in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors("AllowAll") 
// Minimal API Endpoints


// Controllers & Auth
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API is running");
app.MapGet("/healthz", () => Results.Ok("Healthy"));
app.MapGet("/test", async (NpgsqlDataSource db) =>
{
    await using var cmd = db.CreateCommand("SELECT 1");
    return Results.Ok(await cmd.ExecuteScalarAsync());
});
app.Run();