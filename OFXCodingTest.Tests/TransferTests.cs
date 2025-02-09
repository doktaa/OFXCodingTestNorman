using Moq;
using OFXCodingTest.Models;
using OFXCodingTest.Models.DTO.Quote;
using OFXCodingTest.Models.DTO.Transfer;
using OFXCodingTest.Services.Quotes;
using OFXCodingTest.Services.Rates;
using OFXCodingTest.Services.Repository;
using OFXCodingTest.Services.Transfers;

namespace OFXCodingTest.Tests
{
    public class TransferTests
    {
        private TransfersService TransfersService { get; set; }
        private readonly Guid ExistingQuoteId = Guid.NewGuid();
        private readonly Guid NonExistingQuoteId = Guid.NewGuid();
        public TransferTests()
        {
            var mockQuotesService = new Mock<IQuotesService>();
            mockQuotesService
                .Setup(t => t.GetQuote(ExistingQuoteId))
                .ReturnsAsync(new RetrieveQuoteDto() { });

            mockQuotesService
                .Setup(t => t.GetQuote(NonExistingQuoteId))
                .Throws(new InvalidOperationException());

            TransfersService = new TransfersService(
                    mockQuotesService.Object,
                    new InMemoryRepositoryService());
        }
        [Fact]
        public async Task CreateTransfer_CanCreateNewValidTransfer()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = ExistingQuoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "394934",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            // Act
            var createdTransferDto = await TransfersService.CreateTransfer(newCreateTransferDto);


            // Assert
            Assert.NotNull(createdTransferDto);
        }

        [Fact]
        public async Task CreateTransfer_CreatingNewTransferHasCorrectStatus()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = ExistingQuoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "394934",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            // Act
            var createdTransferDto = await TransfersService.CreateTransfer(newCreateTransferDto);


            // Assert
            Assert.NotNull(createdTransferDto);
            Assert.Equal(TransferStatus.Processing, createdTransferDto.Status);
        }

        [Fact]
        public async Task CreateTransfer_CreatingNewTransferHasCorrectDate()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = ExistingQuoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "394934",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            // Act
            var result = await TransfersService.CreateTransfer(newCreateTransferDto);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(DateTime.UtcNow.AddDays(1).Date, result.EstimatedDeliveryDate.Date);
        }

        [Fact]
        public async Task CreateTransfer_ThrowsValidationErrorWhenAccountNumberIsNonNumeric()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = ExistingQuoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "39494d34",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(() => TransfersService.CreateTransfer(newCreateTransferDto));
        }

        [Fact]
        public async Task CreateTransfer_ThrowsValidationErrorWhenQuoteIdIsEmptyGuid()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = Guid.Empty,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "394934",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(() => TransfersService.CreateTransfer(newCreateTransferDto));
        }

        [Fact]
        public async Task CreateTransfer_ThrowsValidationErrorWhenQuoteIdDoesNotExist()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = NonExistingQuoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "394934",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(() => TransfersService.CreateTransfer(newCreateTransferDto));
        }

        [Fact]
        public async Task CreateTransfer_ThrowsValidationErrorWhenMissingInformation()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = Guid.NewGuid(),
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "",
                    TransferReason = ""
                },
                Recipient = new Recipient
                {
                    Name = "",
                    AccountNumber = "",
                    BankCode = "",
                    BankName = ""
                }
            };

            // Act
            await Assert.ThrowsAsync<InvalidDataException>(() => TransfersService.CreateTransfer(newCreateTransferDto));
        }

        [Fact]
        public async Task GetTransfer_CanRetrieveExistingTransfer()
        {
            // Arrange
            var newCreateTransferDto = new CreateTransferDto
            {
                QuoteId = ExistingQuoteId,
                Payer = new Payer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    TransferReason = "Hello"
                },
                Recipient = new Recipient
                {
                    Name = "John Smith",
                    AccountNumber = "394934",
                    BankCode = "sseoifj",
                    BankName = "CBA"
                }
            };

            var createdTransferDto = await TransfersService.CreateTransfer(newCreateTransferDto);

            // Act
            var retrievedTransferDto = await TransfersService.GetTransfer(createdTransferDto.Id);

            // Assert
            Assert.NotNull(retrievedTransferDto);
            Assert.Equal(createdTransferDto.Id, retrievedTransferDto.Id);
        }

        [Fact]
        public async Task GetTransfer_ThrowsValidationErrorWhenTransferDoesNotExist()
        {
            // Arrange
            var invalidTransferId = Guid.NewGuid();

            // Act Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => TransfersService.GetTransfer(invalidTransferId));
        }
    }
}