using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers {
    [Route("api/profit")]
    [ApiController]
    public class ProfitController : ControllerBase {
        private readonly PersonRepository _personRepository;
        private readonly CurrencyRepository _currencyRepository;

        public ProfitController(PersonRepository personRepository, CurrencyRepository currencyRepository) {
            _personRepository = personRepository;
            _currencyRepository = currencyRepository;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ProfitDTO>> GetProfit(int Id) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            CurrencyEntity? currency = new CurrencyEntity();
            if (user == null) return NotFound("Nem létező felhasználó");
            if (user.Wallet.Currencies.Count == 0) return NotFound("Nincs valuta a pénztárcában");
            float buyingPrice = 0;
            float marketPrice = 0;
            float quantity = 0;
            float profitSum = 0;
            foreach (var item in user.Wallet.Currencies) {
                currency = await _currencyRepository.GetCurrencyAsync(item.CryptoId);
                if (currency == null) return NotFound("Valuta nem létezik");
                buyingPrice = item.Price;
                marketPrice = currency.Price;
                quantity = item.Quantity;
                profitSum += (float)(marketPrice - buyingPrice) * quantity;
            }

            ProfitDTO profit = new ProfitDTO { profit = profitSum };
            return Ok(profit);
        }

        [HttpGet("details/{Id}")]
        public async Task<ActionResult<List<ProfitDetailedDTO>>> GetProfitDetails(int Id) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            CurrencyEntity? currency = new CurrencyEntity();
            if (user == null) return NotFound("Nem létező felhasználó");
            if (user.Wallet.Currencies.Count == 0) return NotFound("Nincs valuta a pénztárcában");
            List<ProfitDetailedDTO> profitList = new List<ProfitDetailedDTO>();
            foreach (var item in user.Wallet.Currencies) {
                currency = await _currencyRepository.GetCurrencyAsync(item.CryptoId);
                if (currency == null) return NotFound("Valuta nem létezik");
                profitList.Add(new ProfitDetailedDTO {
                    CryptoName = item.Name,
                    Profit = (float)(currency.Price - item.Price) * item.Quantity
                });
            }
            return Ok(profitList);
        }
    }
}
