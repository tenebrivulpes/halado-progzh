using CryptoBeadando.Models;

namespace CryptoBeadando.DTO {
    public class GetUserDTO {
        public string Username { get; set; }
        public string Email { get; set; }
        public WalletDTO Wallet { get; set; }
    }
}
