using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloomTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsiderController : ControllerBase
    {
        private readonly IInsiderRepository _insiderRepository;

        public InsiderController(IInsiderRepository insiderRepository)
        {
            _insiderRepository = insiderRepository;
        }

        // GET: api/insider/MRNA
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetTrades(string symbol)
        {
            var trades = await _insiderRepository.GetTradesAsync(symbol);
            return Ok(trades);
        }
    }
}