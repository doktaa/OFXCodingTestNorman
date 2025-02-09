using OFXCodingTest.Models.DTO.Quote;
using OFXCodingTest.Services.Quotes;
using OFXCodingTest.Services.Rates;
using OFXCodingTest.Services.Repository;

namespace OFXCodingTest.Tests
{
    public class QuoteTests
    {
        private QuotesService QuoteService { get; set; }
        public QuoteTests() {
            QuoteService = new QuotesService(
                    new InMemoryRatesService(),
                    new InMemoryRepositoryService());
        }
        [Fact]
        public async Task CreateQuote_CanCreateNewValidQuote()
        {
            // Arrange
            var sellCurrency = "AUD";
            var buyCurrency = "USD";
            var amount = 5m;

            var newCreateQuoteDto = new CreateQuoteDto
            {
                SellCurrency = sellCurrency,
                BuyCurrency = buyCurrency,
                Amount = amount
            };

            // Act
            var createdQuoteDto = await QuoteService.CreateQuote(newCreateQuoteDto);


            // Assert
            Assert.NotNull(createdQuoteDto);
        }

        [Theory]
        [InlineData("", "Test")]
        [InlineData("Test", "Test")]
        [InlineData("", "")]
        public async Task CreateQuote_ThrowsValidationErrorWhenFieldsAreMissing(string sellCurrency, string buyCurrency)
        {
            // Arrange
            var amount = 5m;

            var newCreateQuoteDto = new CreateQuoteDto
            {
                SellCurrency = sellCurrency,
                BuyCurrency = buyCurrency,
                Amount = amount
            };

            // Act Assert
            await Assert.ThrowsAsync<InvalidDataException>(() => QuoteService.CreateQuote(newCreateQuoteDto));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public async Task CreateQuote_ThrowsValidationErrorWhenAmountIsLessThanZero(decimal amount)
        {
            // Arrange
            var sellCurrency = "AUD";
            var buyCurrency = "USD";

            var newCreateQuoteDto = new CreateQuoteDto
            {
                SellCurrency = sellCurrency,
                BuyCurrency = buyCurrency,
                Amount = amount
            };

            // Act Assert
            await Assert.ThrowsAsync<InvalidDataException>(() => QuoteService.CreateQuote(newCreateQuoteDto));
        }

        [Theory]
        [InlineData("AUD", "JPY")]
        [InlineData("JPY", "USD")]
        [InlineData("JPY", "EUR")]
        public async Task CreateQuote_ThrowsValidationErrorWhenUsingUnsupportedCurrencies(string sellCurrency, string buyCurrency)
        {
            // Arrange
            var amount = 5m;

            var newCreateQuoteDto = new CreateQuoteDto
            {
                SellCurrency = sellCurrency,
                BuyCurrency = buyCurrency,
                Amount = amount
            };

            // Act Assert
            await Assert.ThrowsAsync<InvalidDataException>(() => QuoteService.CreateQuote(newCreateQuoteDto));
        }

        [Fact]
        public async Task CreateQuote_ThrowsValidationErrorWhenBuyAndSellCurrenciesAreIdentical()
        {
            // Arrange
            var sellCurrency = "USD";
            var buyCurrency = "USD";
            var amount = 5m;

            var newCreateQuoteDto = new CreateQuoteDto
            {
                SellCurrency = sellCurrency,
                BuyCurrency = buyCurrency,
                Amount = amount
            };

            // Act Assert
            await Assert.ThrowsAsync<InvalidDataException>(() => QuoteService.CreateQuote(newCreateQuoteDto));
        }

        [Fact]
        public async Task GetQuote_CanRetrieveExistingQuote()
        {
            // Arrange
            var sellCurrency = "AUD";
            var buyCurrency = "USD";
            var amount = 5m;

            var newCreateQuoteDto = new CreateQuoteDto
            {
                SellCurrency = sellCurrency,
                BuyCurrency = buyCurrency,
                Amount = amount
            };

            var createdQuoteDto = await QuoteService.CreateQuote(newCreateQuoteDto);

            Assert.NotNull(createdQuoteDto);

            // Act
            var retrievedQuote = await QuoteService.GetQuote(createdQuoteDto.Id);

            // Assert
            Assert.NotNull(retrievedQuote);
            Assert.Equal(createdQuoteDto.Id, retrievedQuote.Id);
        }

        [Fact]
        public async Task GetQuote_ThrowsValidationErrorWhenQuoteDoesNotExist()
        {
            // Arrange
            var invalidQuoteId = Guid.NewGuid();

            // Act Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => QuoteService.GetQuote(invalidQuoteId));
        }
    }
}