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
    [Route("api/crypto")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly CurrencyRepository _currencyRepository;

        public CryptoController(CurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [HttpPut("price")]
        public async Task<IActionResult> UpdatePrice([FromBody] NewCurrencyPriceDTO newPrice)
        {
            if (newPrice.Price <= 0) return BadRequest("Ár nem lehet 0 vagy negatív!");
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(newPrice.Id);
            if (currency == null) return NotFound("Nem létezik a valuta");
            await _currencyRepository.UpdateCurrencyPriceAsync(currency, newPrice.Price);
            return Ok();
        }

        [HttpGet("price/history/{Id}")]
        public async Task<ActionResult<List<HistoryDTO>>> GetPriceHistory(int Id)
        {
            CurrencyEntity? currency = await _currencyRepository.GetCurrencyAsync(Id);
            if (currency == null) return NotFound("Nem létezik árfolyam napló");
            List<HistoryDTO> history = new List<HistoryDTO>();
            history.AddRange(currency.History.Select(h => new HistoryDTO
            {
                Name = currency.Name,
                Price = h.Price,
                Date = h.Date
            }));
            return Ok(history);
        }
    }
}
