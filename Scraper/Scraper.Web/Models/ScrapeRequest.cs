using System.Collections.Generic;

namespace Scraper.Web.Models
{
    /// <summary>
    /// Simple object for wrapping scrape requests.
    /// </summary>
    public class ScrapeRequest
    {
        /// <summary>
        /// Url to scrape.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// List of selectors to pull out.
        /// </summary>
        public IEnumerable<string> Selectors { get; set; }
    }
}