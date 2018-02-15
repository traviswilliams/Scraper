using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper.Services;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace Scraper.Tests
{
    [TestClass]
    public class HttpClientScraperTests
    {
        IScraperService Scraper { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Scraper = new SimpleScraperService();
        }

        [TestMethod]
        public void Scraper_GetResultCorrectSelectors()
        {
            var result = Scraper.Scrape("https://www.google.com", new List<string> { "a", "a.class-name", "div" });

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Body);
            Assert.AreEqual(3, result.Scrape.Keys.Count);
        }

        [TestMethod]
        public void Scraper_GetResultWithoutSelectors()
        {
            var result = Scraper.Scrape("https://www.google.com", Enumerable.Empty<string>());

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Body);
        }

        [TestMethod]
        public void Scraper_BadUrl()
        {
            var result = Scraper.Scrape("bad_url", new List<string> { });
            Assert.IsNotNull(result.Error);
        }
    }
}
