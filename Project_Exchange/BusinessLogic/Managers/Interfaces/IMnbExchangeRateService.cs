using BusinessLogic.Models;

namespace BusinessLogic.Managers.Interfaces
{
    public interface IMnbExchangeRateService
    {
        Task<IReadOnlyCollection<ExchangeRate>> GetCurrentRatesAsync(CancellationToken cancellationToken = default);
    }
}

