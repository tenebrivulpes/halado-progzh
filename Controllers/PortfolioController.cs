using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly PersonRepository _personRepository;
        private readonly CurrencyRepository _currencyRepository;

        public PortfolioController(PersonRepository personRepository, CurrencyRepository currencyRepository)
        {
            _personRepository = personRepository;
            _currencyRepository = currencyRepository;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<List<PortfolioDTO>>> GetTransactions(int Id)
        {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound("Felhasználó nem létezik");
            List<PortfolioDTO> currencies = new List<PortfolioDTO>();
            foreach (var currency in user.Wallet.Currencies)
            {
                CurrencyEntity? currencyEntity = await _currencyRepository.GetCurrencyAsync(currency.CryptoId);
                if (currencyEntity == null) return NotFound("Valuta nem létezik");
                currencies.Add(new PortfolioDTO
                {
                    Name = currency.Name,
                    Quantity = currency.Quantity,
                    Price = currency.Price,
                    TotalValue = currency.Price * currency.Quantity,
                    MarketPrice = currencyEntity.Price,
                    MarketValue = currencyEntity.Price * currency.Quantity
                });
            }
            return Ok(currencies);
        }
    }
}

