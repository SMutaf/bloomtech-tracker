using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloomTech.Core.Entities
{
    [Table("InsiderTrades")]
    public class InsiderTrade
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string Name { get; set; } = string.Empty; // İşlemi yapan kişi
        public string Type { get; set; } = string.Empty; // Buy, Sell, Option Exercise
        public long Shares { get; set; } // İşlem adedi
        public decimal Amount { get; set; } // İşlem tutarı (dolar)
        public DateTime TransactionDate { get; set; } // İşlem tarihi
        public string Url { get; set; } = string.Empty;
    }
}
