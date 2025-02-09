using OFXCodingTest.Models.DTO.Quote;

namespace OFXCodingTest.Services.Quotes
{
    public interface IQuotesService
    {
        public Task<RetrieveQuoteDto> CreateQuote(CreateQuoteDto createQuoteDto);
        public Task<RetrieveQuoteDto> GetQuote(Guid quoteId);
    }
}
