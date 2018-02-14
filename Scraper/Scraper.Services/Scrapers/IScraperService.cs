using System.Threading.Tasks;

namespace Scraper.Services
{
    public interface IScraperService
    {
        ScrapeResult Scrape(string url);

        Task<ScrapeResult> ScrapeAsync(string url);
    }
}
