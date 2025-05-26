using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers {
    [Route("api/cryptos")]
    [ApiController]
    public class CurrencyController : ControllerBase {
        private readonly CurrencyRepository _currencyRepository;

        public CurrencyController(CurrencyRepository currencyRepository) {
            _currencyRepository = currencyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<GetCurrencyDTO>> GetCryptos() {
            List<CurrencyEntity> currencyList = await _currencyRepository.GetCurrenciesAsync();
            if (currencyList == null) return NotFound();
            List<GetCurrencyDTO> getCurrencies = new List<GetCurrencyDTO>();
            foreach (var item in currencyList)
                getCurrencies.Add(new GetCurrencyDTO { Name = item.Name, Price = item.Price });
            return Ok(getCurrencies);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<GetCurrencyDTO>> GetCrypto(int Id) {
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(Id);
            if (currency == null) return NotFound();
            GetCurrencyDTO getCurrency = new GetCurrencyDTO { Name = currency.Name, Price = currency.Price };
            return Ok(getCurrency);
        }
        [HttpPost]
        public async Task<IActionResult> NewCurrency([FromBody] GetCurrencyDTO currency) {
            if (string.IsNullOrEmpty(currency.Name)) return BadRequest("Név nem lehet üres!");
            if (currency.Price <= 0) return BadRequest("Ár nem lehet 0 vagy negatív!");
            await _currencyRepository.AddCurrencyAsync(new CurrencyEntity
            {
                Name = currency.Name,
                Price = currency.Price
            });
        return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCurrency(int Id)
        {
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(Id);
            if (currency == null) return NotFound();
            await _currencyRepository.DeleteCurrency(currency);
            return Ok();
        }
    }
}
