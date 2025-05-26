namespace CryptoBeadando.Models {
    public class LimitTransactionEntity {
        public int Id { get; set; }
        public PersonEntity Person { get; set; }
        public CurrencyEntity Currency { get; set; }
        public float Quantity { get; set; }
        public float LimitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Buying { get; set; }
        public bool Active { get; set; } = true;
    }
}
