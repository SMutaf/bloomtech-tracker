using BloomTech.Core.Entities;

namespace BloomTech.Core.Interfaces
{
    public interface IInsiderService
    {
        Task<List<InsiderTrade>> GetLatestTradesAsync(string symbol, string cik);
    }
}