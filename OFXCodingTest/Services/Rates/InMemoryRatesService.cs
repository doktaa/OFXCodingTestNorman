using OFXCodingTest.Models;

namespace OFXCodingTest.Services.Rates
{
    public class InMemoryRatesService : IRatesService
    {
        public Task<Rate> GetRate(string sell, string buy)
        {
            // Assumes that currencies entering this service are valid ??
            var rateResult = MockDataStore.Rates.Where(r => r.BuyCurrency == buy && r.SellCurrency == sell);

            if (!rateResult.Any()) {
                throw new Exception("Rates not found for this buy/sell currency configuration");
            }

            return Task.FromResult(rateResult.Single());
        }
    }
}
