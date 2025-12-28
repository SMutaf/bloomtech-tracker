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

        // Hangi şirkete ait? (Foreign Key)
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public decimal Price { get; set; }
        public long Volume { get; set; } // Hacim büyük olabilir, long kullanıyoruz
        public DateTime Timestamp { get; set; } // Verinin çekildiği an
    }
}
