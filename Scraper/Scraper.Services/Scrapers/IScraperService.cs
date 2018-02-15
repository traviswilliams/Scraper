using Scraper.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Services
{
    public interface IScraperService
    {
        ScrapeResult Scrape(string url, IEnumerable<string> selectors);

        Task<ScrapeResult> ScrapeAsync(string url, IEnumerable<string> selectors);
    }
}
