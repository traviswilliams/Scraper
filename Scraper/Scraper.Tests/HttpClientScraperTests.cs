using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper.Services;
using System.Net;

namespace Scraper.Tests
{
    [TestClass]
    public class HttpClientScraperTests
    {
        IScraperService Scraper { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Scraper = new HttpClientScraper();
        }

        [TestMethod]
        public void Scraper_GetResult()
        {
            var result = Scraper.Scrape("https://www.google.com");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Body);
        }

        [TestMethod]
        public void Scraper_BadUrl()
        {
            var result = Scraper.Scrape("bad_url");
            Assert.IsNotNull(result.Error);
        }
    }
}
