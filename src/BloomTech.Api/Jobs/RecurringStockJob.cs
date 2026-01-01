using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using BloomTech.Data.Context;

namespace BloomTech.Api.Jobs
{
    public class RecurringStockJob
    {
        private readonly IFinanceService _financeService;
        private readonly IStockRepository _stockRepository;
        private readonly AppDbContext _context; // Company ID bulmak için

        // Dependency Injection: İhtiyacımız olan aletleri istiyoruz
        public RecurringStockJob(IFinanceService financeService, IStockRepository stockRepository, AppDbContext context)
        {
            _financeService = financeService;
            _stockRepository = stockRepository;
            _context = context;
        }

        // Hangfire bu metodu çalıştıracak
        public async Task ProcessStockData()
        {
            string symbol = "MRNA"; // Takip ettiğimiz şirket

            // 1. Önce şirketin ID'sini bul (Veritabanında kayıtlı olmalı)
            var company = _context.Companies.FirstOrDefault(c => c.Symbol == symbol);

            // Eğer şirket yoksa (ilk çalışmada), otomatik oluşturalım
            if (company == null)
            {
                company = new Company { Symbol = symbol, Name = "Moderna Inc.", Sector = "Biotechnology" };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
            }

            // 2. Yahoo Finance'den güncel veriyi çek
            try
            {
                var stockData = await _financeService.GetRealTimeDataAsync(symbol);

                // 3. Veriyi şirketle ilişkilendir ve kaydet
                stockData.CompanyId = company.Id;

                await _stockRepository.AddDataAsync(stockData);

                Console.WriteLine($"[BAŞARILI] {DateTime.Now}: {symbol} fiyatı ({stockData.Price}) kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HATA] Veri çekilemedi: {ex.Message}");
            }
        }
    }
}