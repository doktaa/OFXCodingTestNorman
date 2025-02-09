using OFXCodingTest.Models;

namespace OFXCodingTest
{
    public static class MockDataStore
    {
        public static Dictionary<Guid, Quote> Quotes { get; set; } = new Dictionary<Guid, Quote>();

        public static Dictionary<Guid, Transfer> Transfers { get; set; } = new Dictionary<Guid, Transfer>();

        public static HashSet<string> SupportedSellCurrencies { get; set; } = new HashSet<string>
        {
            "AUD",
            "USD",
            "EUR"
        };

        public static HashSet<string> SupportedBuyCurrencies { get; set; } = new HashSet<string>
        {
            "USD",
            "INR",
            "PHP"
        };

        public static List<Rate> Rates { get; } = new List<Rate>()
        {
            new Rate
            {
                SellCurrency = "AUD",
                BuyCurrency = "USD",
                OfxRate = 0.6m,
                InverseOfxRate = 1.52m
            },
            new Rate
            {
                SellCurrency = "AUD",
                BuyCurrency = "INR",
                OfxRate = 52.12m,
                InverseOfxRate = 0.02m
            },
            new Rate
            {
                SellCurrency = "AUD",
                BuyCurrency = "PHP",
                OfxRate = 33.82m,
                InverseOfxRate = 0.03m
            },
            new Rate
            {
                SellCurrency = "USD",
                BuyCurrency = "INR",
                OfxRate = 83.07m,
                InverseOfxRate = 0.01m
            },
            new Rate
            {
                SellCurrency = "USD",
                BuyCurrency = "PHP",
                OfxRate = 53.9m,
                InverseOfxRate = 0.02m
            },
            new Rate
            {
                SellCurrency = "EUR",
                BuyCurrency = "USD",
                OfxRate = 0.99m,
                InverseOfxRate = 0.92m
            },
            new Rate
            {
                SellCurrency = "EUR",
                BuyCurrency = "INR",
                OfxRate = 85.8m,
                InverseOfxRate = 0.01m
            },
            new Rate
            {
                SellCurrency = "EUR",
                BuyCurrency = "PHP",
                OfxRate = 55.67m,
                InverseOfxRate = 0.02m
            }
        };
    }
}

// improvements :
// supported buy/sell currency
// Upgrade validation so it returns all invalid reasons instead of just one
// Validation should probably be moved to their own validator classes/helpers (or IsValid() methods in the DTO classes?)
// Global exception handling
// Move all error messages to constants

// mapper between model and DTO's so don't have to manually keep mapping


// assumptions:
// account number is a string but must contain numerics only
// business logic: payer ID when creating a transfer doesn't need to be existing payer, and we are not implementing the concept of a payer entity
// we will cache buy-sell rate results, excluding time of spotrate and amount. Caching rate datetime as well as amount converted will mean there's a very low chacne that the cached data gets re-accessed
