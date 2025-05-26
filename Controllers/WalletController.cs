using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers {
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : ControllerBase {
        private readonly WalletRepository _walletRepository;
        private readonly PersonRepository _personRepository;

        public WalletController(WalletRepository walletRepository, PersonRepository personRepository) {
            _walletRepository = walletRepository;
            _personRepository = personRepository;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<WalletDTO>> GetWallet(int Id) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound();
            List<CurrencyDTO> currencies = new List<CurrencyDTO>();
            foreach (var item in user.Wallet.Currencies)
                currencies.Add(new CurrencyDTO { Name = item.Name, Price = item.Price, Quantity = item.Quantity });
            return Ok(new WalletDTO { Balance = user.Wallet.Balance, Currencies = currencies });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> ModifyWallet(int Id, [FromBody] float newBalance){
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound();
            if (newBalance < 0) return BadRequest("A pénzösszeg nem lehet negatív!");
            await _walletRepository.UpdateWAlletAsync(user, newBalance);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteWallet(int Id) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound();
            await _walletRepository.DeleteWalletAsync(user);
            return Ok();
        }
    }
}
