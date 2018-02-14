using Scraper.Services;
using System.Net;
using System.Threading.Tasks;

namespace Scraper.Tests.Mocks
{
    public class MockScraper : IScraperService
    {
        public ScrapeResult Scrape(string url)
        {
            return ScrapeAsync(url).GetAwaiter().GetResult();
        }

        public async Task<ScrapeResult> ScrapeAsync(string url)
        {
            return await Task.Run(() => new ScrapeResult { Body = url, StatusCode = HttpStatusCode.OK });
        }
    }
}
