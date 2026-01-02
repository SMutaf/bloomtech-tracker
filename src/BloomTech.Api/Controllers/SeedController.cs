using BloomTech.Core.Entities;
using BloomTech.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BloomTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public SeedController(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // Bu linke gidince çalışacak: /api/Seed/MrnaHistory
        [HttpGet("MrnaHistory")]
        public async Task<IActionResult> SeedHistory()
        {
            string symbol = "MRNA";
            // range=1y (1 Yıl), range=5y (5 Yıl) yapabilirsin.
            // interval=1d (Günlük veri)
            string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?range=1y&interval=1d";

            // Header temizliği ve eklemesi
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var json = JObject.Parse(response);

                var result = json["chart"]["result"][0];
                var timestamps = result["timestamp"].ToObject<long[]>(); // Tarihler (Unix formatında)
                var indicators = result["indicators"]["quote"][0]; // Fiyatlar

                var opens = indicators["open"].ToObject<decimal?[]>();
                var highs = indicators["high"].ToObject<decimal?[]>();
                var lows = indicators["low"].ToObject<decimal?[]>();
                var closes = indicators["close"].ToObject<decimal?[]>(); // Kapanış (Price)
                var volumes = indicators["volume"].ToObject<long?[]>();

                // 1. Şirketi bul
                var company = _context.Companies.FirstOrDefault(c => c.Symbol == symbol);
                if (company == null) return BadRequest("HATA: Önce Hangfire Job'ı bir kere çalıştırıp şirketin oluşmasını sağla.");

                // 2. TEMİZLİK: Mevcut verileri silelim ki grafik tertemiz olsun (Duplicate olmasın)
                var oldData = _context.StockData.Where(s => s.CompanyId == company.Id);
                _context.StockData.RemoveRange(oldData);
                await _context.SaveChangesAsync();

                // 3. YENİ VERİLERİ EKLE
                int count = 0;
                for (int i = 0; i < timestamps.Length; i++)
                {
                    // Bazı günler borsa tatildir, veri null gelir. Onları atla.
                    if (closes[i] == null) continue;

                    var stockPrice = new StockData
                    {
                        CompanyId = company.Id,
                        // Unix zaman damgasını normal tarihe çevir
                        Timestamp = DateTimeOffset.FromUnixTimeSeconds(timestamps[i]).DateTime,
                        Price = closes[i].Value,
                        Open = opens[i] ?? 0,
                        High = highs[i] ?? 0,
                        Low = lows[i] ?? 0,
                        Volume = volumes[i] ?? 0
                    };
                    _context.StockData.Add(stockPrice);
                    count++;
                }

                await _context.SaveChangesAsync();
                return Ok($"✅ BAŞARILI! Toplam {count} adet geçmiş gün verisi yüklendi. Şimdi grafiğe bakabilirsin.");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Hata oluştu: {ex.Message}");
            }
        }
    }
}