using System.Text.Json.Serialization;

namespace OFXCodingTest.Models.DTO.Quote
{
    public class RetrieveQuoteDto
    {
        [JsonPropertyName("quoteId")]
        public Guid Id { get; set; }
        public decimal OfxRate { get; set; }
        public decimal InverseOfxRate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}
