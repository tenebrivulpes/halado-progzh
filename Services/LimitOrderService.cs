using CryptoBeadando.Models;
using CryptoBeadando.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CryptoBeadando.Services {
    public class LimitOrderService : BackgroundService {

        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(15); // 15 másodperc

        public LimitOrderService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _serviceProvider.CreateScope())
                {
                    var limitTransactionRepository = scope.ServiceProvider.GetRequiredService<LimitTransactionRepository>();
                    var currencyRepository = scope.ServiceProvider.GetRequiredService<CurrencyRepository>();
                    var personRepository = scope.ServiceProvider.GetRequiredService<PersonRepository>();

                    await ProcessLimitOrders(limitTransactionRepository, currencyRepository, personRepository);
                    await Task.Delay(_updateInterval, stoppingToken);
                }
            }
        }

        private async Task ProcessLimitOrders(LimitTransactionRepository limitTransactionRepository,
            CurrencyRepository currencyRepository, PersonRepository personRepository) {
            var limitTransactions = await limitTransactionRepository.GetLimitTransactionsAsync();

            foreach (var transaction in limitTransactions) {
                CurrencyEntity? currency = await currencyRepository.GetCurrencyAsync(transaction.Currency.Id);
                if (currency == null) continue;

                //Vásárlás
                if (transaction.Buying && currency.Price <= transaction.LimitPrice) {
                    transaction.Active = false;

                    PersonEntity? user = await personRepository.GetPersonAsync(transaction.Person.Id);
                    WalletCurrencyEntity? walletCurrency = user.Wallet.Currencies.FirstOrDefault(c => c.CryptoId == currency.Id);

                    if (user.Wallet.Balance >= (currency.Price * transaction.Quantity)) {

                        if (walletCurrency == null)
                        {
                            user.Wallet.Currencies.Add(new WalletCurrencyEntity
                            {
                                Name = currency.Name,
                                CryptoId = currency.Id,
                                Price = currency.Price,
                                Quantity = transaction.Quantity
                            });
                        }
                        else
                        {
                            walletCurrency.Price = ((currency.Price * walletCurrency.Quantity) +
                                (transaction.LimitPrice * transaction.Quantity)) / (walletCurrency.Quantity + transaction.Quantity);
                            walletCurrency.Quantity += transaction.Quantity;
                        }
                        user.Wallet.Balance -= currency.Price * transaction.Quantity;
                        await limitTransactionRepository.DeleteLimitTransactionAsync(transaction);
                        await personRepository.UpdatePersonWallet(user);
                    }
                }

                //Eladás
                if (!transaction.Buying && currency.Price >= transaction.LimitPrice) {
                    transaction.Active = false;

                    PersonEntity? user = await personRepository.GetPersonAsync(transaction.Person.Id);
                    WalletCurrencyEntity? walletCurrency = user.Wallet.Currencies.FirstOrDefault(c => c.CryptoId == currency.Id);

                    if (walletCurrency != null && walletCurrency.Quantity >= transaction.Quantity) {

                        walletCurrency.Quantity -= transaction.Quantity;
                        if (walletCurrency.Quantity == 0)
                        {
                            user.Wallet.Currencies.Remove(walletCurrency);
                        }

                        user.Wallet.Balance += currency.Price * transaction.Quantity;
                        await limitTransactionRepository.DeleteLimitTransactionAsync(transaction);
                        await personRepository.UpdatePersonWallet(user);
                    }
                }

                //Dátum ellenőrzése
                if (transaction.ExpirationDate < DateTime.Now)
                {
                    transaction.Active = false;
                    await limitTransactionRepository.DeleteLimitTransactionAsync(transaction);
                }
            }
        }
    }
}
