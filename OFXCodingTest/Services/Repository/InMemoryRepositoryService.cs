using OFXCodingTest.Models;

namespace OFXCodingTest.Services.Repository
{
    public class InMemoryRepositoryService : IRepositoryService
    {
        public async Task<Quote> AddQuote(Quote newQuote)
        {
            if (MockDataStore.Quotes.ContainsKey(newQuote.Id))
            {
                throw new InvalidOperationException($"Quote with ID {newQuote.Id} already exists in the system");
            }

            MockDataStore.Quotes[newQuote.Id] = newQuote;

            var createdQuote = await GetQuote(newQuote.Id);

            return createdQuote;
        }

        public Task<Quote> GetQuote(Guid quoteId)
        {
            if (!MockDataStore.Quotes.ContainsKey(quoteId))
            {
                throw new InvalidOperationException($"Quote with ID {quoteId} not found");
            }

            var retrievedQuote = MockDataStore.Quotes[quoteId];
            return Task.FromResult(retrievedQuote);
        }

        public async Task<Transfer> AddTransfer(Transfer newTransfer)
        {
            if (MockDataStore.Transfers.ContainsKey(newTransfer.Id))
            {
                throw new InvalidOperationException($"Transfer with ID {newTransfer.Id} already exists in the system");
            }

            MockDataStore.Transfers[newTransfer.Id] = newTransfer;

            var createdTransfer = await GetTransfer(newTransfer.Id);

            return createdTransfer;
        }

        public Task<Transfer> GetTransfer(Guid transferId)
        {
            if (!MockDataStore.Transfers.ContainsKey(transferId))
            {
                throw new InvalidOperationException($"Transfer with ID {transferId} not found");
            }

            var retrievedTransfer = MockDataStore.Transfers[transferId];
            return Task.FromResult(retrievedTransfer);
        }
    }
}
