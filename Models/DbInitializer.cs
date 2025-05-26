using CryptoBeadando.DTO;
using CryptoBeadando.Models;

public static class DbInitializer {
    public static void Seed(CryptoDbContext context) {
        if (!context.Currencies.Any()) {
            var defaultCurrencies = new List<CurrencyEntity> {
                new CurrencyEntity { Name = "Firstcoin", Price = 30000 },
                new CurrencyEntity { Name = "Secondeum", Price = 2000 },
                new CurrencyEntity { Name = "Thirdcoin", Price = 100 },
                new CurrencyEntity { Name = "Fourthium", Price = 500},
                new CurrencyEntity { Name = "Fifthcash", Price = 15000 },
                new CurrencyEntity { Name = "Sixthbit", Price = 250},
                new CurrencyEntity { Name = "Seventhcoin", Price = 8000 },
                new CurrencyEntity { Name = "Eighthium", Price = 1200 },
                new CurrencyEntity { Name = "Ninthcash", Price = 450},
                new CurrencyEntity { Name = "Tenthbit", Price = 7000},
                new CurrencyEntity { Name = "Eleventhcoin", Price = 350 },
                new CurrencyEntity { Name = "Twelfthium", Price = 1800 },
                new CurrencyEntity { Name = "Thirteenthcash", Price = 220 },
                new CurrencyEntity { Name = "Fourteenthbit", Price = 9500 },
                new CurrencyEntity { Name = "Fifteenthcoin", Price = 400 }
            };

            var defaultUsers = new List<PersonEntity> {
                new PersonEntity {Name = "József", Email = "joci@email.com", Password = "jocipassword", Wallet = new WalletEntity { Balance = 81000.0f } },
                new PersonEntity {Name = "Anna", Email = "anna@email.com", Password = "annapassword", Wallet = new WalletEntity { Balance = 81000.0f } },
                new PersonEntity {Name = "Béla", Email = "bela@email.com", Password = "belapassword", Wallet = new WalletEntity { Balance = 81000.0f } },
                new PersonEntity {Name = "Katalin", Email = "katalin@email.com", Password = "katalinpassword", Wallet = new WalletEntity { Balance = 81000.0f } },
                new PersonEntity {Name = "Dávid", Email = "david@email.com", Password = "davidpassword", Wallet = new WalletEntity { Balance = 81000.0f } },
                new PersonEntity {Name = "Eszter", Email = "eszter@email.com", Password = "eszterpassword", Wallet = new WalletEntity { Balance = 81000.0f } }
            };

            var defaultWallets = defaultUsers.Select(u => u.Wallet).ToList();

            var defaultTransactions = new List<TransactionEntity> {
                new TransactionEntity { Person = defaultUsers[0], Currency = defaultCurrencies[12], Quantity = 20, Price = 220, Date = DateTime.Now, Buying = true },
                new TransactionEntity { Person = defaultUsers[0], Currency = defaultCurrencies[13], Quantity = 5, Price = 9500, Date = DateTime.Now, Buying = true },
                new TransactionEntity { Person = defaultUsers[0], Currency = defaultCurrencies[12], Quantity = 4, Price = 220, Date = DateTime.Now, Buying = false },
                new TransactionEntity { Person = defaultUsers[0], Currency = defaultCurrencies[13], Quantity = 2, Price = 9500, Date = DateTime.Now, Buying = true }
            };

            foreach (var currency in defaultCurrencies) {
                currency.History.Add(new CurrencyHistoryEntity { Price = currency.Price, Date = DateTime.Now });
                context.History.Add(currency.History[0]);
            }

            context.Currencies.AddRange(defaultCurrencies);
            context.Users.AddRange(defaultUsers);
            context.Wallets.AddRange(defaultWallets);
            context.Transactions.AddRange(defaultTransactions);
            context.SaveChanges();
        }
    }
}
