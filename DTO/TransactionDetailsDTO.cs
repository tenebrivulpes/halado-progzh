namespace CryptoBeadando.DTO {
    public class TransactionDetailsDTO {
        public string CryptoName { get; set; }
        public string Type { get; set; } // "Vétel" vagy "Eladás"
        public float Quantity { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
    }
}
