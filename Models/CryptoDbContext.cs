using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System;

namespace CryptoBeadando.Models {
    public class CryptoDbContext : DbContext {
        public CryptoDbContext(DbContextOptions options) : base(options) {}

        public DbSet<PersonEntity> Users { get; set; }
        public DbSet<WalletEntity> Wallets { get; set; }
        public DbSet<WalletCurrencyEntity> WalletCurrencies { get; set; }
        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<CurrencyHistoryEntity> History { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<LimitTransactionEntity> LimitTransactions { get; set; }

    }
}
