using Microsoft.Extensions.Caching.Memory;
using OFXCodingTest.Models;
using OFXCodingTest.Models.DTO;
using System.Text.Json;

namespace OFXCodingTest.Services.Rates
{
    public class PublicRatesService : IRatesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        public PublicRatesService(
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache
        ) { 
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }
        public async Task<Rate> GetRate(string sell, string buy)
        {
            // Check cache
            var cacheKey = $"{sell}-{buy}";

            if (_cache.TryGetValue(cacheKey, out Rate? cachedRate))
            {
                return cachedRate!;
            }

            // Request rates
            var request = GenerateOfxRateRequest(sell, buy);

            var httpClient = _httpClientFactory.CreateClient();

            var httpResponseMessage = await httpClient.SendAsync(request);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var rateResult = await JsonSerializer.DeserializeAsync
                    <PublicApiRateDto>(contentStream);

                if (rateResult == null)
                {
                    throw new Exception("Could not retrieve rates from public API");
                }

                var rateToReturn = new Rate()
                {
                    BuyCurrency = buy,
                    SellCurrency = sell,
                    OfxRate = rateResult.OfxRate,
                    InverseOfxRate = rateResult.InverseOfxRate
                };

                // Add to cache
                AddToCache(cacheKey, rateToReturn);

                return rateToReturn;
            }

            throw new Exception("Could not retrieve rates from public API");
        }

        public HttpRequestMessage GenerateOfxRateRequest(string sell, string buy)
        {
            var uri = new Uri($"https://api.ofx.com/PublicSite.ApiService/OFX/spotrate/Individual/{sell}/{buy}/1");

            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = HttpMethod.Get
            };

            return request;
        }

        public void AddToCache(string key, Rate value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(key, value, cacheEntryOptions);
        }
    }
}
