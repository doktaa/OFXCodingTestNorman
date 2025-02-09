using OFXCodingTest.Models.DTO.Quote;
using OFXCodingTest.Models;
using OFXCodingTest.Models.DTO.Transfer;
using OFXCodingTest.Services.Rates;
using OFXCodingTest.Services.Repository;
using OFXCodingTest.Services.Quotes;
using System.Text.RegularExpressions;

namespace OFXCodingTest.Services.Transfers
{
    public class TransfersService : ITransfersService
    {
        private readonly IQuotesService _quotesService;
        private readonly IRepositoryService _repositoryService;
        public TransfersService(
            IQuotesService quotesService,
            IRepositoryService repositoryService
        ) {
            _quotesService = quotesService;
            _repositoryService = repositoryService;
        }
        public async Task<RetrieveTransferDto> CreateTransfer(CreateTransferDto createTransferDto)
        {
            // Validate Transfer DTO - throws exception if fails
            await ValidateCreateTransfer(createTransferDto);

            // Add to mock store
            var estimatedDeliveryDate = DateTime.UtcNow;
            estimatedDeliveryDate = estimatedDeliveryDate.AddDays(1);

            var newTransfer = new Transfer()
            {
                Id = Guid.NewGuid(), //initialise new GUID here as we don't have ORM/DB integration to automatically determine PK upon insert
                Status = TransferStatus.Processing, // As per test specs, the respone of a transfer creation should have status set to Processing instead of Created
                QuoteId = createTransferDto.QuoteId,
                Payer = createTransferDto.Payer,
                Recipient = createTransferDto.Recipient,
                EstimatedDeliveryDate = estimatedDeliveryDate
            };

            var createdTransfer = await _repositoryService.AddTransfer(newTransfer);

            return createdTransfer.MapTransferToRetrieveTransferDto();
        }

        public async Task<RetrieveTransferDto> GetTransfer(Guid transferId)
        {
            var retrievedTransfer = await _repositoryService.GetTransfer(transferId);

            return retrievedTransfer.MapTransferToRetrieveTransferDto();
        }

        private async Task ValidateCreateTransfer(CreateTransferDto transfer)
        {
            // All fields required
            if (transfer.QuoteId == Guid.Empty)
            {
                throw new InvalidDataException("QuoteId must not be an empty ID");
            } else if (transfer.Payer.Id == Guid.Empty)
            {
                throw new InvalidDataException("Payer ID must not be an empty ID");
            }

            if (
                string.IsNullOrEmpty(transfer.Payer.Name) ||
                string.IsNullOrEmpty(transfer.Payer.TransferReason) ||
                string.IsNullOrEmpty(transfer.Recipient.Name) ||
                string.IsNullOrEmpty(transfer.Recipient.AccountNumber) ||
                string.IsNullOrEmpty(transfer.Recipient.BankCode) ||
                string.IsNullOrEmpty(transfer.Recipient.BankName)) // Can probably iterate through properties and check for empty strings
            {
                throw new InvalidDataException("Data must not contain missing or empty strings");
            }

            // check account number contains numbers only
            Regex validateNumberRegex = new Regex("^\\d+$");
            if (!validateNumberRegex.IsMatch(transfer.Recipient.AccountNumber))
            {
                throw new InvalidDataException("Account number must contain numbers only");
            }

            // Check quote ID is valid existing quote
            await _quotesService.GetQuote(transfer.QuoteId);
        }
    }
}
