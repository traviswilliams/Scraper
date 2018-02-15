using System;
using System.Collections.Generic;
using System.Net;

namespace Scraper.Services
{
    public class SourceResult   
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Body { get; set; }

        public Exception Error { get; set; }
    }
}
