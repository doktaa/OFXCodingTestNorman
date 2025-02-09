using OFXCodingTest.Models.DTO.Transfer;

namespace OFXCodingTest.Models
{
    public class Transfer
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public Guid QuoteId { get; set; }
        public Payer Payer { get; set; }
        public Recipient Recipient { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public RetrieveTransferDto MapTransferToRetrieveTransferDto()
        {
            return new RetrieveTransferDto
            {
                Id = Id,
                Status = Status,
                TransferDetails = new TransferDetailsDto
                {
                    QuoteId = QuoteId,
                    Payer = Payer,
                    Recipient = Recipient
                },
                EstimatedDeliveryDate = EstimatedDeliveryDate
            };
        }
    }

    public static class TransferStatus
    {
        public static string Created = "Created";
        public static string Processing = "Processing";
        public static string Processed = "Processed";
        public static string Failed = "Failed";
    }
}
