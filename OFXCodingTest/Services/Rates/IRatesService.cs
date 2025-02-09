using OFXCodingTest.Models;

namespace OFXCodingTest.Services.Rates
{
    public interface IRatesService
    {
        public Task<Rate> GetRate(string sell, string buy);
    }
}
