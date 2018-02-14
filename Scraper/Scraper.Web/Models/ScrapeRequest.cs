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
    }
}