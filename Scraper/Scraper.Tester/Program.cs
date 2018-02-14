using Newtonsoft.Json;
using Scraper.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scraper.Tester
{
    /// <summary>
    /// This applications purpose is to load test the API for 1 minute.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            const string localIISUrl = "http://localhost:65304";

            var scrapeUrl = "/scraper/scrape";

            var pageScrapeList = new[]
            {
                "http://localhost:65304/Api/POST-scraper-scrape",
                "http://localhost:65304/Api/GET-scraper-job-id",
                "http://localhost:65304/Api/GET-scraper-pending",
                "http://localhost:65304/Api/GET-scraper-running",
                "http://localhost:65304/Api/GET-scraper-completed",
                "http://localhost:65304/Api/GET-scraper-failed",
                "http://localhost:65304/Api/GET-admin-status",
                "http://localhost:65304/Api/POST-admin-start",
                "http://localhost:65304/Api/POST-admin-stop",
                "http://localhost:65304/Api/POST-admin-pause",
                "http://localhost:65304/Api/POST-admin-resume",
                "http://localhost:65304/Api/POST-admin-concurrency-maxConcurrency",
                "http://localhost:65304/Api/GET-admin-concurrency",
                "malformed_url",
                "http://someplacethatshouldnotexist.anotherunlikelyspot.io"
            };

            var endTime = DateTime.UtcNow.AddMinutes(1);

            var jobs = new List<Job>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(localIISUrl);

                while (endTime >= DateTime.Now)
                {

                    Parallel.ForEach(pageScrapeList, (url) =>
                    {
                        var content = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("url", url)
                        });
                        var response = client.PostAsync(scrapeUrl, content).GetAwaiter().GetResult();

                        if (response.IsSuccessStatusCode)
                        {
                            var job = JsonConvert.DeserializeObject<Job>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                            Console.WriteLine($"Job: {job.Id} -- Status: {job.Status} -- Url: {job.Url}");
                        }
                        else
                        {
                            Console.WriteLine($"Error loading: {url} -- http code: {response.StatusCode}\r\n{response.ReasonPhrase}");
                        }
                    });
                }
            }

            Console.WriteLine();
            Console.WriteLine("Enter to exit.");

            Console.ReadLine();
        }
    }
}
