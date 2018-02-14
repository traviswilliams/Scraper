using Newtonsoft.Json;
using Scraper.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Scraper.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            const string localIISUrl = "http://localhost:65304";

            var scrapeUrl = $"/scraper/scrape";

            var pageScrapeList = new[]
            {
                "https://www.wellsfargo.com",
                "https://www.chase.com",
                "https://www.google.com",
                "https://www.yahoo.com",
                "https://www.schwab.com",
                "https://www.etrade.com",
                "https://www.tdameritrade.com",
                "https://www.stackoverflow.com",
                "https://www.yahoo.com",
                "https://www.msn.com"
            };

            var jobs = new List<IJob>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(localIISUrl);

                foreach(var url in pageScrapeList)
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("url", url)
                    });
                    var response = client.PostAsync(scrapeUrl, content).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                        jobs.Add(JsonConvert.DeserializeObject<Job>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()));
                    else
                        Console.WriteLine($"Error loading: {url} -- http code: {response.StatusCode}\r\n{response.ReasonPhrase}");
                }
            }

            foreach(var job in jobs)
            {
                Console.WriteLine($"Job: {job.Id} -- Status: {job.Status} -- Url: {job.Url}");
            }

            Console.WriteLine();
            Console.WriteLine("Enter to exit.");

            Console.ReadLine();
        }
    }
}
