using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scraper.Services
{
    public class HttpClientScraper : IScraperService
    {
        public ScrapeResult Scrape(string url)
        {
            return ScrapeAsync(url)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<ScrapeResult> ScrapeAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url);
                    return new ScrapeResult
                    {
                        StatusCode = result.StatusCode,
                        Body = result.IsSuccessStatusCode 
                            ? await result.Content.ReadAsStringAsync()
                            : string.Empty,
                        Error = result.IsSuccessStatusCode
                            ? null
                            : new Exception(result.ReasonPhrase)
                    };
                }
            }
            catch(Exception ex)
            {
                return new ScrapeResult
                {
                    Error = ex
                };
            }
        }
    }
}
