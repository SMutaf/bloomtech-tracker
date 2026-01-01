using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomTech.Data.Context;
using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BloomTech.Data.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockData>> GetHistoryAsync(string symbol)
        {
            return await _context.StockData
                .Include(s => s.Company) // Join işlemi
                .Where(s => s.Company.Symbol == symbol)
                .OrderByDescending(s => s.Timestamp)
                .ToListAsync();
        }

        public async Task AddDataAsync(StockData data)
        {
            await _context.StockData.AddAsync(data);
            await _context.SaveChangesAsync();
        }
    }
}
