using System.Text.Json.Serialization;

namespace OFXCodingTest.Models.DTO
{
    public class PublicApiRateDto
    {
        [JsonPropertyName("InterbankRate")]
        public decimal OfxRate { get; set; }
        [JsonPropertyName("InverseInterbankRate")]
        public decimal InverseOfxRate { get; set; }
    }
}
