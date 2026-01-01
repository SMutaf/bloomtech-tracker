using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace BloomTech.Data.Services
{
    public class YahooFinanceService : IFinanceService
    {
        private readonly HttpClient _httpClient;

        public YahooFinanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StockData> GetRealTimeDataAsync(string symbol)
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?interval=1d";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var json = JObject.Parse(response);

                var result = json["chart"]["result"][0];
                var price = result["meta"]["regularMarketPrice"].Value<decimal>();
                var volume = result["meta"]["regularMarketVolume"]?.Value<long>() ?? 0;

                return new StockData
                {
                    Price = price,
                    Volume = volume,
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yahoo Finance Hatası: {ex.Message}");
                throw;
            }
        }
    }
}