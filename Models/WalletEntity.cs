namespace CryptoBeadando.Models {
    public class WalletEntity {
        public int Id { get; set; }
        public float Balance { get; set; }
        public List<WalletCurrencyEntity> Currencies { get; set; } = new List<WalletCurrencyEntity>();
    }
}
