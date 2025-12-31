using BloomTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomTech.Core.Interfaces
{
    public interface IFinanceService
    {
        // verilen sembol verisini çeker
        Task<StockData> GetRealTimeDataAsync(string symbol);
    }
}
