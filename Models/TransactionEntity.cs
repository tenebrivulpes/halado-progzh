namespace CryptoBeadando.Models {
    public class TransactionEntity {
        public int Id {  get; set; }
        public PersonEntity Person { get; set; }
        public CurrencyEntity Currency { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public bool Buying { get; set; }
    }
}
