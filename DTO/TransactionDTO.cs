namespace CryptoBeadando.DTO {
    public class TransactionDTO {
        public int Id { get; set; }
        public string CryptoName { get; set; }
        public string Type { get; set; } // "Vétel" vagy "Eladás"
    }
}
