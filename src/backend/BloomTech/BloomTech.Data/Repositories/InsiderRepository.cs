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
    public class InsiderRepository : IInsiderRepository
    {
        private readonly AppDbContext _context;

        public InsiderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<InsiderTrade>> GetTradesAsync(string symbol)
        {
            return await _context.InsiderTrades
                .Include(i => i.Company)
                .Where(i => i.Company.Symbol == symbol)
                .OrderByDescending(i => i.TransactionDate)
                .ToListAsync();
        }

        public async Task AddTradeAsync(InsiderTrade trade)
        {
            await _context.InsiderTrades.AddAsync(trade);
            await _context.SaveChangesAsync();
        }
    }
}
