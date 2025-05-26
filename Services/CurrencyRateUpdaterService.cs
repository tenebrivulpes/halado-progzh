using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Models;

namespace CryptoBeadando.Services
{
    public class CurrencyRateUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(30); // 30 másodperc

        public CurrencyRateUpdaterService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();

                    await UpdateCurrencyRates(dbContext);

                    await Task.Delay(_updateInterval, stoppingToken);
                }
            }
        }

        private async Task UpdateCurrencyRates(CryptoDbContext dbContext)
        {
            var currencies = await dbContext.Currencies.ToListAsync();

            foreach (var currency in currencies)
            {
                currency.Price = GenerateNewPrice(currency.Price);

                dbContext.Update(currency);

                currency.History.Add(new CurrencyHistoryEntity
                {
                    Price = currency.Price,
                    Date = DateTime.Now
                });

                dbContext.History.Add(currency.History[^1]); // hozzáadja a legújabb árat
            }

            await dbContext.SaveChangesAsync();
        }

        private float GenerateNewPrice(float currentPrice)
        {
            Random random = new Random();
            float change = (float)(random.NextDouble() * 2 - 1); // -1% és +1% között
            return currentPrice * (1 + change / 100);
        }
    }
}
