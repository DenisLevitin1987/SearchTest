using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SearchEngine.Searcher;

namespace SearchWeb.Controllers
{

    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearcher _searcher;

        public SearchController(ISearcher searcher, ILogger<SearchController> logger)
        {
            _logger = logger;
            _searcher = searcher;
        }

        [HttpGet("laptops")]
        public ActionResult<SearchResponse> Get(string query)
        {
            var result = _searcher.Search(query);
            return new SearchResponse()
            {
                Laptops = result.Laptops.Select(x => new LaptopDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Attributes = x.Attributes
                }).ToArray()
            };
        }
    }
}