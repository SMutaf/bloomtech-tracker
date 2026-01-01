using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloomTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        // Dependency Injection ile Repository'i içeri alıyoruz
        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        // GET: api/stock/history/MRNA
        // Mimari Madde 5.2: Fiyat ve metrikleri getiren endpoint
        [HttpGet("history/{symbol}")]
        public async Task<IActionResult> GetHistory(string symbol)
        {
            var data = await _stockRepository.GetHistoryAsync(symbol);
            return Ok(data);
        }

        // POST: api/stock
        // Test amaçlı veya manuel veri girişi için
        [HttpPost]
        public async Task<IActionResult> AddStockData([FromBody] StockData stockData)
        {
            await _stockRepository.AddDataAsync(stockData);
            return CreatedAtAction(nameof(GetHistory), new { symbol = stockData.CompanyId }, stockData);
        }
    }
}