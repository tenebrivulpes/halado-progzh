﻿namespace CryptoBeadando.Models {
    public class PersonEntity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public WalletEntity Wallet { get; set; }
    }
}
