using Scraper.Services;
using Scraper.Services.Models;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Scraper.Tests.Mocks
{
    public class MockScraper : IScraperService
    {
        public ScrapeResult Scrape(string url, IEnumerable<string> selectors)
        {
            return ScrapeAsync(url, selectors).GetAwaiter().GetResult();
        }

        public async Task<ScrapeResult> ScrapeAsync(string url, IEnumerable<string> selectors)
        {
            var selectorList = selectors != null && selectors.Any() 
                ? selectors.ToList() 
                : new List<string> { "html" };

            return await Task.Run(() => new ScrapeResult
            {
                Body = url,
                StatusCode = HttpStatusCode.OK,
                Scrape = selectorList.ToDictionary(s => s, s => Enumerable.Empty<string>())
            });
        }
    }
}
