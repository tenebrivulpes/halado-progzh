using CryptoBeadando.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoBeadando.Repository {
    public class LimitTransactionRepository(CryptoDbContext context) {
        private readonly DbSet<LimitTransactionEntity> limitTransactions = context.Set<LimitTransactionEntity>();

        public async Task<List<LimitTransactionEntity>> GetLimitTransactionsAsync(int userId) {
            return await limitTransactions.Where(t => t.Active == true).Include(t => t.Person)
                .Include(t => t.Currency).Where(t => t.Person.Id == userId).ToListAsync();
        }

        public async Task<List<LimitTransactionEntity>> GetLimitTransactionsAsync()
        {
            return await limitTransactions.Where(t => t.Active == true).Include(t => t.Person)
                .Include(t => t.Currency).ToListAsync();
        }

        public async Task<LimitTransactionEntity> GetLimitTransactionAsync(int Id) {
            return await limitTransactions.Include(t => t.Currency).FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task AddLimitTransactionAsync(LimitTransactionEntity transaction) {
            await limitTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
        }

        public async Task DeleteLimitTransactionAsync(LimitTransactionEntity transaction) {
            limitTransactions.Remove(transaction);
            await context.SaveChangesAsync();
        }
    }
}
