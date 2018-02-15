using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Scraper.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scraper.Services
{
    public class SimpleScraperService : IScraperService
    {
        public SimpleScraperService()
        {

        }

        public ScrapeResult Scrape(string html, IEnumerable<string> selectors)
        {
            return ScrapeAsync(html, selectors)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<ScrapeResult> ScrapeAsync(string url, IEnumerable<string> selectors)
        {
            var source = await GetSourceAsync(url);
            var scrapeResult = new ScrapeResult
            {
                StatusCode = source.StatusCode,
                Body = source.Body,
                Error = source.Error
            };

            if (scrapeResult.Error != null)
                return scrapeResult;

            var selectorList = selectors != null && selectors.Any() 
                ? selectors.ToList() 
                : new List<string> { "html" };

            var results = new ConcurrentDictionary<string, IEnumerable<string>>();

            var parser = new HtmlParser();

            using (var parsedHtml = await parser.ParseAsync(source.Body))
            {
                Parallel.ForEach(selectorList, (selector) =>
                {
                    var result = ParseSelector(parsedHtml, selector);

                    if (results.ContainsKey(selector))
                    {
                        results[selector].Union(result);
                    }
                    else
                    {
                        results.TryAdd(selector, result);
                    }
                });
            }

            scrapeResult.Scrape = results;

            return scrapeResult;
        }

        private async Task<SourceResult> GetSourceAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url);
                    return new SourceResult
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
            catch (Exception ex)
            {
                return new SourceResult { Error = ex };
            }
        }

        private List<string> ParseSelector(IHtmlDocument parsedHtml, string selector)
        {
            var selection = parsedHtml.QuerySelectorAll(selector);

            var results = new List<string>();

            foreach (var item in selection)
            {
                results.Add(item.TextContent);
            }

            return results;
        }
    }
}
