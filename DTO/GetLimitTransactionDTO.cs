namespace CryptoBeadando.DTO {
    public class GetLimitTransactionDTO {
        public float LimitPrice { get; set; }
        public float Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string TransactionType { get; set; } // "Vétel" or "Eladás"
    }
}
