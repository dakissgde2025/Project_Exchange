namespace BusinessLogic.Models
{
    public class ExchangeRate
    {
        public string Currency { get; set; } = string.Empty;
        public int Unit { get; set; }
        public decimal Value { get; set; }
    }
}

