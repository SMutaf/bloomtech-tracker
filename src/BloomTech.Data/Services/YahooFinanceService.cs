using BloomTech.Core.Entities; // StockData sınıfının olduğu yer (DTO veya Entity)
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
            // Header kontrolü (Hata vermemesi için güvenli ekleme)
            if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            }

            string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?interval=1d";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var json = JObject.Parse(response);

                var result = json["chart"]["result"][0];
                var meta = result["meta"]; // Veriler buranın içinde!

                var price = meta["regularMarketPrice"]?.Value<decimal>() ?? 0;
                var volume = meta["regularMarketVolume"]?.Value<long>() ?? 0;
                var open = meta["regularMarketOpen"]?.Value<decimal>() ?? 0;
                var high = meta["regularMarketDayHigh"]?.Value<decimal>() ?? 0;
                var low = meta["regularMarketDayLow"]?.Value<decimal>() ?? 0;

                return new StockData
                {
                    Price = price,
                    Volume = volume,
                    Open = open,
                    High = high,
                    Low = low,
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