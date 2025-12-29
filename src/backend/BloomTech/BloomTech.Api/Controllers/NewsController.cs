using BloomTech.Core.Entities;
using BloomTech.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloomTech.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        // GET: api/news/MRNA
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetNews(string symbol)
        {
            var news = await _newsRepository.GetNewsAsync(symbol);
            return Ok(news);
        }
    }
}