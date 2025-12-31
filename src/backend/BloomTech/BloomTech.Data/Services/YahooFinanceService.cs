using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace BloomTech.Data.Services
{
    public class YahooFinanceService : IFinanceService
    {
        private readonly HttpClient _httpClient;

        public YahooFinanceService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<StockData> GetRealTimeDataAsync(string symbol)
        {
            // Yahoo Finance'in resmi olmayan JSON endpoint'i
            // Bu URL, hisse senedi verisini JSON olarak döner.
            string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?interval=1d";

            try
            {
                // 1. İsteği at
                var response = await _httpClient.GetStringAsync(url);

                // 2. JSON'u parse et
                var json = JObject.Parse(response);

                // 3. Karmaşık JSON içinden fiyatı bul (Path: chart.result[0].meta.regularMarketPrice)
                var result = json["chart"]["result"][0];
                var price = result["meta"]["regularMarketPrice"].Value<decimal>();
                var volume = result["meta"]["regularMarketVolume"]?.Value<long>() ?? 0;

                // 4. StockData nesnesini oluştur ve döndür
                return new StockData
                {
                    Price = price,
                    Volume = volume,
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                // Hata olursa loglanabilir, şimdilik null veya hata fırlatıyoruz
                Console.WriteLine($"Yahoo Finance Hatası: {ex.Message}");
                throw;
            }
        }
    }
}