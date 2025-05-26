namespace CryptoBeadando.Models
{
    public class WalletCurrencyEntity
    {
        public int Id { get; set; }
        public int CryptoId { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public float Quantity { get; set; }
    }
}
