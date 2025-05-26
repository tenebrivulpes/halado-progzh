namespace CryptoBeadando.DTO {
    public class LimitTransactionDTO {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public float Quantity { get; set; }
        public float LimitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
