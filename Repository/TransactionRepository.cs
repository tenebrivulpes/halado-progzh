using CryptoBeadando.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoBeadando.Repository {
    public class TransactionRepository(CryptoDbContext context) {
        private readonly DbSet<TransactionEntity> transactions = context.Set<TransactionEntity>();

        public async Task<List<TransactionEntity>> GetTransactionsAsync(int userId) {
            return await transactions.Include(t => t.Person).Include(t => t.Currency).Where(t => t.Person.Id == userId).ToListAsync();
        }

        public async Task<TransactionEntity> GetTransactionAsync(int Id) {
            return await transactions.Include(t => t.Currency).FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task AddTransactionAsync(TransactionEntity transaction) {
            await transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
        }
    }
}
