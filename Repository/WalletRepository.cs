using CryptoBeadando.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoBeadando.Repository {
    public class WalletRepository(CryptoDbContext context) {
        private readonly DbSet<WalletEntity> wallets = context.Set<WalletEntity>();

        public async Task<WalletEntity> GetWalletAsync(int id) {
            return await wallets.Include(u => u.Currencies)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateWAlletAsync(PersonEntity user, float newBal) {
            user.Wallet.Balance = newBal;
            await context.SaveChangesAsync();
        }

        public async Task DeleteWalletAsync(PersonEntity user) {
            user.Wallet = new WalletEntity { Balance = 0.0f };
            await context.SaveChangesAsync();
        }

    }
}
