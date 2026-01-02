using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloomTech.Core.Entities
{
    [Table("SupplyChain")]
    public class SupplyChain
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string SupplierSymbol { get; set; } = string.Empty; 
        public string SupplierName { get; set; } = string.Empty;   
        public string Relation { get; set; } = string.Empty;      
    }
}
