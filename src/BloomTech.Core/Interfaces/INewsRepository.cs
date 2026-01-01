using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomTech.Core.Entities;

namespace BloomTech.Core.Interfaces
{
    public interface INewsRepository
    {
        // Şirket haberlerini getir
        Task<List<News>> GetNewsAsync(string symbol);

        // Haber var mı diye kontrol et (URL'ye göre - mükerrer kaydı önlemek için)
        Task<bool> ExistsAsync(string url);

        // Toplu haber kaydet
        Task AddRangeAsync(List<News> newsList);
    }
}