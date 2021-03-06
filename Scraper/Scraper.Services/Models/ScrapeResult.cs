﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Scraper.Services.Models
{
    public class ScrapeResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Body { get; set; }

        public IDictionary<string, IEnumerable<string>> Scrape { get; set; } = new Dictionary<string, IEnumerable<string>>();

        public Exception Error { get; set; }
    }
}
