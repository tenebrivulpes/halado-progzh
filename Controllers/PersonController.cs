using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CryptoBeadando.Models;
using System;
using System.ComponentModel.Design.Serialization;
using CryptoBeadando.DTO;
using Microsoft.EntityFrameworkCore;
using CryptoBeadando.Repository;

namespace CryptoBeadando.Controllers {

    [Route("api/users")]
    [ApiController]
    public class PersonController : ControllerBase {

        private readonly PersonRepository _personRepository;

        public PersonController( PersonRepository personRepository) {
            _personRepository = personRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register) {
            PersonEntity? user = await _personRepository.GetPersonAsync(register.Email);
            if (user != null) return BadRequest("Létezik az email cím!");
            await _personRepository.AddPersonAsync(register.Username, register.Email, register.Password);
            return Ok();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(int Id) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound();
            List<CurrencyDTO> currencies = new List<CurrencyDTO>();
            foreach (var item in user.Wallet.Currencies)
                currencies.Add(new CurrencyDTO { Name = item.Name, Price = item.Price, Quantity = item.Quantity });
            WalletDTO wallet = new WalletDTO { Balance = user.Wallet.Balance, Currencies = currencies };
            return Ok(new GetUserDTO { Username = user.Name, Email = user.Email, Wallet = wallet });
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> ModifyUser(int Id, [FromBody] RegisterDTO newData) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound();
            await _personRepository.UpdatePerson(user, newData.Username, newData.Email, newData.Password);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(int Id) {
            PersonEntity? user = await _personRepository.GetPersonAsync(Id);
            if (user == null) return NotFound();
            await _personRepository.DeletePersonAsync(user);
            return Ok();
        }


    }
}
