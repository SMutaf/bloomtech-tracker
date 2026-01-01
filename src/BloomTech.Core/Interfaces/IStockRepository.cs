using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomTech.Core.Entities;

namespace BloomTech.Core.Interfaces
{
    public interface IStockRepository
    {
        // Belirli bir şirketin tüm fiyat geçmişini getir
        Task<List<StockData>> GetHistoryAsync(string symbol);

        // Yeni fiyat verisi ekle (Background Job kullanacak)
        Task AddDataAsync(StockData data);
    }
}
