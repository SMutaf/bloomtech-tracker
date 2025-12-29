using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using BloomTech.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomTech.Api.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly AppDbContext _context;

        public NewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<News>> GetNewsAsync(string symbol)
        {
            return await _context.News
                .Include(n => n.Company)
                .Where(n => n.Company.Symbol == symbol)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string url)
        {
            return await _context.News.AnyAsync(n => n.Url == url);
        }

        public async Task AddRangeAsync(List<News> newsList)
        {
            await _context.News.AddRangeAsync(newsList);
            await _context.SaveChangesAsync();
        }
    }
}
