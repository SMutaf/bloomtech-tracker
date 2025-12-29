using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomTech.Core.Entities;

namespace BloomTech.Core.Interfaces
{
    public interface IInsiderRepository
    {
        // Şirketin insider işlemlerini getir
        Task<List<InsiderTrade>> GetTradesAsync(string symbol);

        // Yeni işlem kaydet
        Task AddTradeAsync(InsiderTrade trade);
    }
}