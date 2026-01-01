using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using BloomTech.Data.Context;

namespace BloomTech.Api.Jobs
{
    public class RecurringInsiderJob
    {
        private readonly IInsiderService _insiderService;
        private readonly IInsiderRepository _insiderRepository;
        private readonly AppDbContext _context;

        public RecurringInsiderJob(IInsiderService insiderService, IInsiderRepository insiderRepository, AppDbContext context)
        {
            _insiderService = insiderService;
            _insiderRepository = insiderRepository;
            _context = context;
        }

        public async Task ProcessInsiderTrades()
        {
            string symbol = "MRNA";
            string cik = "0001682852"; // Moderna'nın SEC Kimlik Numarası

            var company = _context.Companies.FirstOrDefault(c => c.Symbol == symbol);
            if (company == null) return;

            // 1. Veriyi Çek
            var trades = await _insiderService.GetLatestTradesAsync(symbol, cik);

            // 2. Basit Mükerrer Kontrolü ve Kayıt
            // (Gerçek projede TransactionId kontrolü yapılır, burada tarihe bakacağız)
            foreach (var trade in trades)
            {
                // Aynı tarihli ve aynı kişili işlem var mı?
                var exists = _context.InsiderTrades.Any(t =>
                    t.TransactionDate == trade.TransactionDate &&
                    t.Name == trade.Name);

                if (!exists)
                {
                    trade.CompanyId = company.Id;
                    await _insiderRepository.AddTradeAsync(trade);
                    Console.WriteLine($"[INSIDER] {trade.Name} işlemi kaydedildi.");
                }
            }
        }
    }
}