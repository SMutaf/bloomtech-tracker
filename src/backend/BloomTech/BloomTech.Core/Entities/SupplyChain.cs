using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloomTech.Core.Entities
{
    // PDF'teki tablo ismi: SupplyChain 
    [Table("SupplyChain")]
    public class SupplyChain
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string SupplierSymbol { get; set; } = string.Empty; // Örn: LNZ
        public string SupplierName { get; set; } = string.Empty;   // Örn: Lonza Group
        public string Relation { get; set; } = string.Empty;       // PDF: relation  (Manufacturing, Packaging vb.)
    }
}
