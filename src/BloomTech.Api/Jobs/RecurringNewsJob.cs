using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using BloomTech.Data.Context;
//using BloomTech.Infrastructure.Data; // Veya BloomTech.Data.Context (Namespace'ine dikkat et)

namespace BloomTech.Api.Jobs
{
    public class RecurringNewsJob
    {
        private readonly INewsService _newsService;
        private readonly INewsRepository _newsRepository;
        private readonly AppDbContext _context;

        public RecurringNewsJob(INewsService newsService, INewsRepository newsRepository, AppDbContext context)
        {
            _newsService = newsService;
            _newsRepository = newsRepository;
            _context = context;
        }

        public async Task ProcessNews()
        {
            string symbol = "MRNA";


            var company = _context.Companies.FirstOrDefault(c => c.Symbol == symbol);
            if (company == null) return; 

            var latestNews = await _newsService.GetLatestNewsAsync(symbol);

            int newCount = 0;

            foreach (var item in latestNews)
            {
                bool exists = await _newsRepository.ExistsAsync(item.Url);

                if (!exists)
                {
                    item.CompanyId = company.Id;
                }
            }

            var newItems = new List<News>();
            foreach (var news in latestNews)
            {
                if (!await _newsRepository.ExistsAsync(news.Url))
                {
                    news.CompanyId = company.Id;
                    newItems.Add(news);
                }
            }

            if (newItems.Any())
            {
                await _newsRepository.AddRangeAsync(newItems);
                Console.WriteLine($"[HABER] {newItems.Count} yeni haber kaydedildi.");
            }
        }
    }
}