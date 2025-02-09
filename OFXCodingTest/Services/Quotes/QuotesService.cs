using OFXCodingTest.Models;
using OFXCodingTest.Models.DTO.Quote;
using OFXCodingTest.Services.Rates;
using OFXCodingTest.Services.Repository;
using System.Linq;

namespace OFXCodingTest.Services.Quotes
{
    public class QuotesService : IQuotesService
    {
        private readonly IRatesService _ratesService;
        private readonly IRepositoryService _repositoryService;
        public QuotesService(
            IRatesService ratesService,
            IRepositoryService repositoryService
        ) {
            _ratesService = ratesService;
            _repositoryService = repositoryService;
        }
        public async Task<RetrieveQuoteDto> CreateQuote(CreateQuoteDto createQuoteDto)
        {
            // Validate Quote DTO - throws exception if fails
            ValidateCreateQuote(createQuoteDto);

            // Retrieve rate
            var rateResult = await _ratesService.GetRate(createQuoteDto.SellCurrency, createQuoteDto.BuyCurrency);

            // Add to mock store
            var newQuote = new Quote()
            {
                Id = Guid.NewGuid(), //initialise new GUID here as we don't have ORM/DB integration to automatically determine PK upon insert
                SellCurrency = createQuoteDto.SellCurrency,
                BuyCurrency = createQuoteDto.BuyCurrency,
                Amount = createQuoteDto.Amount,
                ConvertedAmount = createQuoteDto.Amount * rateResult.OfxRate,
                OfxRate = rateResult.OfxRate,
                InverseOfxRate = rateResult.InverseOfxRate,
                DateCreatedUtc = DateTime.UtcNow
            };

            var createdQuote = await _repositoryService.AddQuote(newQuote);

            return createdQuote.MapQuoteToRetrieveQuoteDto();
        }

        public async Task<RetrieveQuoteDto> GetQuote(Guid quoteId)
        {
            var retrievedQuote = await _repositoryService.GetQuote(quoteId);

            return retrievedQuote.MapQuoteToRetrieveQuoteDto();
        }

        private void ValidateCreateQuote(CreateQuoteDto quote)
        {
            // All fields required, amount must be greater than 0
            if (
                string.IsNullOrEmpty(quote.BuyCurrency) ||
                string.IsNullOrEmpty(quote.SellCurrency) ||
                quote.Amount <= 0)
            {
                throw new InvalidDataException("All fields are required and amount must be greater than 0");
            }

            // Currencies can't be equal
            if (quote.BuyCurrency == quote.SellCurrency)
            {
                throw new InvalidDataException("Buy and sell currencies must not be identical");
            }

            // check currencies are supported for buy/sell
            if (!MockDataStore.SupportedBuyCurrencies.Contains(quote.BuyCurrency))
            {
                throw new InvalidDataException($"{quote.BuyCurrency} is an unsupported BUY currency");
            }

            if (!MockDataStore.SupportedSellCurrencies.Contains(quote.SellCurrency))
            {
                throw new InvalidDataException($"{quote.SellCurrency} is an unsupported SELL currency");
            }
        }
    }
}
