using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomTech.Core.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty; // Örn: MRNA
        public string Name { get; set; } = string.Empty;   // Örn: Moderna Inc.
        public string Sector { get; set; } = string.Empty; // Örn: Biotechnology

        // İlişkiler (Navigation Properties)
        public List<StockData> StockHistory { get; set; } = new();
        public List<News> NewsList { get; set; } = new();
    }
}
