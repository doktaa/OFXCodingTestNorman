using OFXCodingTest.Models;

namespace OFXCodingTest.Services.Repository
{
    public interface IRepositoryService
    {
        public Task<Quote> AddQuote(Quote newQuote);

        public Task<Quote> GetQuote(Guid quoteId);
        public Task<Transfer> AddTransfer(Transfer transfer);

        public Task<Transfer> GetTransfer(Guid transferId);
    }
}
