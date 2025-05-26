using CryptoBeadando.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoBeadando.Repository {
    public class CurrencyRepository(CryptoDbContext context) {
        private readonly DbSet<CurrencyEntity> currencies = context.Set<CurrencyEntity>();
        private readonly DbSet<CurrencyHistoryEntity> history = context.Set<CurrencyHistoryEntity>();

        public async Task<List<CurrencyEntity>> GetCurrenciesAsync() {
            return await currencies.Include(c => c.History)
                .ToListAsync();
        }

        public async Task<CurrencyEntity> GetCurrencyAsync(int id) {
            return await currencies.Include(c => c.History)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCurrencyAsync(CurrencyEntity currency) {
            await currencies.AddAsync(currency);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCurrency(CurrencyEntity currency) {
            context.History.RemoveRange(currency.History);
            currencies.Remove(currency);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCurrencyPriceAsync(CurrencyEntity currency, float newPrice) {
            currency.Price = newPrice;
            context.Update(currency);
            currency.History.Add(new CurrencyHistoryEntity
            {
                Price = currency.Price,
                Date = DateTime.Now
            });
            history.Add(currency.History[^1]);
            await context.SaveChangesAsync();
        }
    }
}
