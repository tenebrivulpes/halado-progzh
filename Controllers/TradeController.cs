using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers {
    [Route("api/trade")]
    [ApiController]
    public class TradeController : ControllerBase {
        private readonly PersonRepository _personRepository;
        private readonly CurrencyRepository _currencyRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly LimitTransactionRepository _limitTransactionRepository;

        public TradeController(PersonRepository personRepository, LimitTransactionRepository limitTransactionRepository,
            TransactionRepository transactionRepository, CurrencyRepository currencyRepository) {
            _personRepository = personRepository;
            _limitTransactionRepository = limitTransactionRepository;
            _transactionRepository = transactionRepository;
            _currencyRepository = currencyRepository;
        }

        [HttpPost("buy")]
        public async Task<ActionResult<TransactionEntity>> BuyCrypto([FromBody] TradeDTO transactionData) {
            PersonEntity? user = await _personRepository.GetPersonAsync(transactionData.UserId);
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(transactionData.CryptoId);
            if (user == null) return NotFound("Felhasználó nem létezik");
            if (currency == null) return NotFound("Valuta nem létezik");
            if (transactionData.Quantity <= 0) return BadRequest("A mennyiségnek nagyobbnak kell lennie mint 0");
            WalletCurrencyEntity? walletCurrency = user.Wallet.Currencies.FirstOrDefault(c => c.CryptoId == transactionData.CryptoId);
            await _transactionRepository.AddTransactionAsync(new TransactionEntity
            {
                Person = user,
                Currency = currency,
                Quantity = transactionData.Quantity,
                Price = currency.Price * transactionData.Quantity,
                Date = DateTime.Now,
                Buying = true
            });
            if (walletCurrency == null) {
                user.Wallet.Currencies.Add(new WalletCurrencyEntity
                {
                    Name = currency.Name,
                    CryptoId = currency.Id,
                    Price = currency.Price * transactionData.Quantity,
                    Quantity = transactionData.Quantity
                });
            } else{
                walletCurrency.Price = ((currency.Price * walletCurrency.Quantity) +
                            (currency.Price * transactionData.Quantity)) / (walletCurrency.Quantity + transactionData.Quantity);
                walletCurrency.Quantity += transactionData.Quantity;
            }
            user.Wallet.Balance -= currency.Price * transactionData.Quantity;
            await _personRepository.UpdatePersonWallet(user);
            return Ok();
        }

        [HttpPost("sell")]
        public async Task<ActionResult<TransactionEntity>> SellCrypto([FromBody] TradeDTO transactionData)
        {
            PersonEntity? user = await _personRepository.GetPersonAsync(transactionData.UserId);
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(transactionData.CryptoId);
            if (user == null) return NotFound("Felhasználó nem létezik");
            if (currency == null) return NotFound("Valuta nem létezik");
            if (transactionData.Quantity <= 0) return BadRequest("A mennyiségnek nagyobbnak kell lennie mint 0");
            WalletCurrencyEntity? walletCurrency = user.Wallet.Currencies.FirstOrDefault(c => c.CryptoId == transactionData.CryptoId);
            if (walletCurrency == null || (walletCurrency.Quantity - transactionData.Quantity) < 0)
                return BadRequest("Nincs elég valuta a pénztárcában");
            await _transactionRepository.AddTransactionAsync(new TransactionEntity
            {
                Person = user,
                Currency = currency,
                Quantity = transactionData.Quantity,
                Price = currency.Price,
                Date = DateTime.Now,
                Buying = false
            });
            walletCurrency.Quantity -= transactionData.Quantity;
            if (walletCurrency.Quantity == 0)
            {
                user.Wallet.Currencies.Remove(walletCurrency);
            }
            user.Wallet.Balance += currency.Price * transactionData.Quantity;
            await _personRepository.UpdatePersonWallet(user);
            return Ok();
        }

        [HttpPost("limit-buy")]
        public async Task<ActionResult<LimitTransactionEntity>> LimitBuyCrypto([FromBody] LimitTransactionDTO transactionData) {
            PersonEntity? user = await _personRepository.GetPersonAsync(transactionData.UserId);
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(transactionData.CryptoId);
            if (user == null) return NotFound("Felhasználó nem létezik");
            if (currency == null) return NotFound("Valuta nem létezik");
            if (transactionData.Quantity <= 0) return BadRequest("A mennyiségnek nagyobbnak kell lennie mint 0");
            if (transactionData.LimitPrice <= 0) return BadRequest("A limit árnak nagyobbnak kell lennie mint 0");
            if (transactionData.LimitPrice > user.Wallet.Balance) return BadRequest("Nincs elég pénz a vásárláshoz");
            await _limitTransactionRepository.AddLimitTransactionAsync(new LimitTransactionEntity
            {
                Person = user,
                Currency = currency,
                Quantity = transactionData.Quantity,
                LimitPrice = transactionData.LimitPrice,
                ExpirationDate = transactionData.ExpirationDate,
                Buying = true
            });
            return Ok();
        }

        [HttpPost("limit-sell")]
        public async Task<ActionResult<LimitTransactionEntity>> LimitSellCrypto([FromBody] LimitTransactionDTO transactionData) {
            PersonEntity? user = await _personRepository.GetPersonAsync(transactionData.UserId);
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(transactionData.CryptoId);
            if (user == null) return NotFound("Felhasználó nem létezik");
            if (currency == null) return NotFound("Valuta nem létezik");
            WalletCurrencyEntity? walletCurrency = user.Wallet.Currencies.FirstOrDefault(c => c.CryptoId == currency.Id);
            if (walletCurrency == null) return NotFound("Nincs ilyen valuta a pénztárcában");
            if (walletCurrency.Quantity < transactionData.Quantity) return BadRequest("Nincs elég valuta a pénztárcában a limit eladásához");
            if (transactionData.Quantity <= 0) return BadRequest("A mennyiségnek nagyobbnak kell lennie mint 0");
            if (transactionData.LimitPrice <= 0) return BadRequest("A limit árnak nagyobbnak kell lennie mint 0");
            if (transactionData.LimitPrice > user.Wallet.Balance) return BadRequest("Nincs elég pénz a vásárláshoz");
            await _limitTransactionRepository.AddLimitTransactionAsync(new LimitTransactionEntity
            {
                Person = user,
                Currency = currency,
                Quantity = transactionData.Quantity,
                LimitPrice = transactionData.LimitPrice,
                ExpirationDate = transactionData.ExpirationDate,
                Buying = false
            });
            return Ok();
        }

        [HttpGet("limit-orders/{userId}")]
        public async Task<ActionResult<GetLimitTransactionDTO>> GetLimitTransactions(int userId) {
            PersonEntity? user = await _personRepository.GetPersonAsync(userId);
            if (user == null) return NotFound("Felhasználó nem létezik");
            List<LimitTransactionEntity> limitTransactions = await _limitTransactionRepository.GetLimitTransactionsAsync(userId);
            if (limitTransactions == null || limitTransactions.Count == 0) return NotFound("Nincsenek limit tranzakciók");
            List<GetLimitTransactionDTO> limitTransactionDTOs = limitTransactions.Select(t => new GetLimitTransactionDTO
            {
                Quantity = t.Quantity,
                LimitPrice = t.LimitPrice,
                ExpirationDate = t.ExpirationDate,
                TransactionType = t.Buying ? "vétel" : "eladás"
            }).ToList();
            return Ok(limitTransactionDTOs);
        }

        [HttpDelete("limit-orders/{orderId}")]
        public async Task<ActionResult> DeleteLimitTransaction(int orderId) {
            LimitTransactionEntity? limitTransaction = await _limitTransactionRepository.GetLimitTransactionAsync(orderId);
            if (limitTransaction == null) return NotFound("A limit tranzakció nem létezik");
            await _limitTransactionRepository.DeleteLimitTransactionAsync(limitTransaction);
            return Ok();
        }
    }
}
