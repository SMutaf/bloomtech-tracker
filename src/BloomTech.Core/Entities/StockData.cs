using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomTech.Core.Entities
{
    public class StockData
    {
        public int Id { get; set; }

        // Hangi şirkete ait (Foreign Key)
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public decimal Price { get; set; }
        public decimal Open { get; set; }   // Açılış
        public decimal High { get; set; }   // Günün En Yükseği
        public decimal Low { get; set; }    // Günün En Düşüğü
        public long Volume { get; set; }
        public DateTime Timestamp { get; set; } // Verinin çekildiği an
    }
}
