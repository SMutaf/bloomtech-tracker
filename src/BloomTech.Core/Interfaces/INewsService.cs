using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomTech.Core.Entities;

namespace BloomTech.Core.Interfaces
{
    public interface INewsService
    {
        Task<List<News>> GetLatestNewsAsync(string symbol);
    }
}
