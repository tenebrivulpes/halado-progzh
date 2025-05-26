using CryptoBeadando.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace CryptoBeadando.Repository {
    public class PersonRepository (CryptoDbContext context) {
        private readonly DbSet<PersonEntity> users = context.Set<PersonEntity>();

        public async Task<PersonEntity> GetPersonAsync(int id) {
            return await users.Include(u => u.Wallet)
                .ThenInclude(u => u.Currencies).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<PersonEntity> GetPersonAsync(string email) {
            return await users.Include(u => u.Wallet)
                .ThenInclude(u => u.Currencies).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdatePerson(PersonEntity user, string username, string email, string password) {
            if (!string.IsNullOrEmpty(username)) user.Name = username;
            if (!string.IsNullOrEmpty(email)) user.Email = email;
            if (!string.IsNullOrEmpty(password)) user.Password = password;
            await context.SaveChangesAsync();
        }
        public async Task UpdatePersonWallet(PersonEntity user) {
            context.Update(user.Wallet);
            context.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(PersonEntity user) {
            users.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task AddPersonAsync(string username, string email, string password) {
            await users.AddAsync(new PersonEntity
            {
                Name = username,
                Email = email,
                Password = password,
                Wallet = new WalletEntity { Balance = 0.0f }
            });
            await context.SaveChangesAsync();
        }
    }
}
