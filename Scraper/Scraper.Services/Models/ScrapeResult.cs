using System;
using System.Net;

namespace Scraper.Services
{
    public class ScrapeResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Body { get; set; }

        public Exception Error { get; set; }
    }
}
