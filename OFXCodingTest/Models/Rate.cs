namespace OFXCodingTest.Models
{
    public class Rate
    {
        public string BuyCurrency { get; set; }
        public string SellCurrency { get; set; }
        public decimal OfxRate { get; set; }
        public decimal InverseOfxRate { get; set; }
    }
}
