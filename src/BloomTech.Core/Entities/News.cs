using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomTech.Core.Entities
{
    public class News
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty; // Yahoo
        public string Url { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }

    }
}
