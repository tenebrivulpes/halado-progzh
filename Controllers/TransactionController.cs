using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers {
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase {
        private readonly TransactionRepository _transactionRepository;
        private readonly PersonRepository _personRepository;
        private readonly CurrencyRepository _currencyRepository;

        public TransactionController(TransactionRepository transactionRepository,
            PersonRepository personRepository, CurrencyRepository currencyRepository) {
            _transactionRepository = transactionRepository;
            _personRepository = personRepository;
            _currencyRepository = currencyRepository;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTransactions(int Id)
        {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound("Nem létezik a felhasználó");
            List<TransactionEntity>? transactions = await _transactionRepository.GetTransactionsAsync(user.Id);
            if (transactions == null) return NotFound("Nincsenek tranzakciók");
            List<TransactionDTO> transactionData = new List<TransactionDTO>();
            foreach (TransactionEntity transaction in transactions)
            {
                transactionData.Add(new TransactionDTO
                {
                    Id = transaction.Id,
                    CryptoName = transaction.Currency.Name,
                    Type = transaction.Buying ? "Vásárlás" : "Eladás"
                });
            }
            return Ok(transactionData);
        }

        [HttpGet("detailed/{Id}")]
        public async Task<ActionResult<TransactionDetailsDTO>> GetTransactionDetails(int Id)
        {
            TransactionEntity? transaction = await _transactionRepository.GetTransactionAsync(Id);
            if (transaction == null) return NotFound("Nem létezik a tranzakció");
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(transaction.Currency.Id);
            if (currency == null) return NotFound("Nem létezik a valuta");
            TransactionDetailsDTO transactionDetails = new TransactionDetailsDTO
            {
                CryptoName = currency.Name,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                Date = transaction.Date,
                Type = transaction.Buying ? "Vásárlás" : "Eladás"
            };
            return Ok(transactionDetails);
        }
    }
}
