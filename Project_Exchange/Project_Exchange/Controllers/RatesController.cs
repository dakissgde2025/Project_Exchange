using BusinessLogic.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Project_Exchange.Models;

namespace Project_Exchange.Controllers
{
    public class RatesController : Controller
    {
        private static readonly string[] HighlightedCurrencies = new[] { "EUR", "USD", "GBP", "CHF", "CZK", "PLN", "JPY" };
        private readonly IMnbExchangeRateService _exchangeRateService;
        private readonly ILogger<RatesController> _logger;

        public RatesController(IMnbExchangeRateService exchangeRateService, ILogger<RatesController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var rates = await _exchangeRateService.GetCurrentRatesAsync(cancellationToken);
            var hasData = rates.Any();

            if (!hasData)
            {
                _logger.LogWarning("Nem érkeztek árfolyam adatok az MNB szolgáltatástól.");
            }

            var orderedRates = rates
                .OrderByDescending(r => HighlightedCurrencies.Contains(r.Currency))
                .ThenBy(r => r.Currency)
                .Select(r => new ExchangeRateRowViewModel
                {
                    Currency = r.Currency,
                    Unit = r.Unit,
                    Value = r.Value
                })
                .ToList();

            var model = new ExchangeRatesPageViewModel
            {
                Rates = orderedRates,
                RetrievedAtUtc = DateTime.UtcNow,
                HasError = !hasData
            };

            return View(model);
        }
    }
}

