namespace Project_Exchange.Models
{
    public class ExchangeRateRowViewModel
    {
        public string Currency { get; set; } = string.Empty;
        public int Unit { get; set; }
        public decimal Value { get; set; }
        public decimal ValuePerOneUnit => Unit == 0 ? 0 : Value / Unit;
    }

    public class ExchangeRatesPageViewModel
    {
        public IReadOnlyCollection<ExchangeRateRowViewModel> Rates { get; set; } = Array.Empty<ExchangeRateRowViewModel>();
        public DateTime RetrievedAtUtc { get; set; }
        public bool HasError { get; set; }
    }
}

