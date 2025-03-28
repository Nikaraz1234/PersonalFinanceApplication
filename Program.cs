using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using PersonalFinanceApplication.Data;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.PasswordHasher;
using PersonalFinanceApplication.Repositories;
using PersonalFinanceApplication.Services;
using Supabase;
using AutoMapper;
using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);

// ===== Configuration =====
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// ===== Kestrel Configuration =====
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});



// ===== Services Configuration =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SupabaseConnection")));


builder.Services.AddScoped<BudgetRepository>();
builder.Services.AddScoped<TransactionRepository>();

builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddMaps(typeof(Program).Assembly);
}).CreateMapper());


builder.Services.AddControllers();




// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.WebHost.UseSetting(WebHostDefaults.ServerUrlsKey, string.Empty);



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
app.UseCors("AllowAll"); // Must be after UseRouting()
app.UseAuthorization();
app.MapControllers();
// Minimal API Endpoints
app.MapGet("/", () => "API is running");
app.MapGet("/healthz", () => Results.Ok("Healthy"));
app.MapGet("/test", async (NpgsqlDataSource db) =>
{
    await using var cmd = db.CreateCommand("SELECT 1");
    return Results.Ok(await cmd.ExecuteScalarAsync());
});

// Controllers & Auth   


app.Run();