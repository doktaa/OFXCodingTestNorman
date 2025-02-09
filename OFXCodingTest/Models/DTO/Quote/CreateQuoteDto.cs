namespace OFXCodingTest.Models.DTO.Quote
{
    public class CreateQuoteDto
    {
        public string SellCurrency { get; set; }
        public string BuyCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}
