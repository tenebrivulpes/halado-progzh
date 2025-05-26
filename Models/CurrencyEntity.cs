namespace CryptoBeadando.Models {
    public class CurrencyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public List<CurrencyHistoryEntity> History { get; set; } = new List<CurrencyHistoryEntity>();
    }
}
