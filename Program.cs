using CryptoBeadando.Models;
using CryptoBeadando.Repository;
using CryptoBeadando.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddDbContext<CryptoDbContext>(options => options.UseSqlServer("Server=(local);Database=CryptoDb_b9r4zn;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddDbContext<CryptoDbContext>(options => options.UseSqlServer("Data Source=DESKTOP-8L00OM7\\SQLEXPRESS;Database=CryptoDb_b9r4zn;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CurrencyRepository>();
builder.Services.AddTransient<PersonRepository>();
builder.Services.AddTransient<TransactionRepository>();
builder.Services.AddTransient<LimitTransactionRepository>();
builder.Services.AddTransient<WalletRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddHostedService<CurrencyRateUpdaterService>();
builder.Services.AddHostedService<LimitOrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    using (var scope = app.Services.CreateScope()) {
        var context = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();
        DbInitializer.Seed(context);
    }
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
