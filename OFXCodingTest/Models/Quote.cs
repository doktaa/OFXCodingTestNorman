using OFXCodingTest.Models.DTO.Quote;

namespace OFXCodingTest.Models
{
    public class Quote
    {
        public Guid Id { get; set; }
        public string SellCurrency { get; set; }
        public string BuyCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public decimal OfxRate { get; set; }
        public decimal InverseOfxRate { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public RetrieveQuoteDto MapQuoteToRetrieveQuoteDto()
        {
            return new RetrieveQuoteDto
            {
                Id = Id,
                OfxRate = OfxRate,
                InverseOfxRate = InverseOfxRate,
                ConvertedAmount = ConvertedAmount
            };
        }
    }
}
