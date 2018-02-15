using Newtonsoft.Json;
using Scraper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scraper.Tester
{
    /// <summary>
    /// This applications purpose is to load test the API for 1 minute.
    /// </summary>
    class Program
    {
        const string localIISUrl = "http://localhost:65304";

        static void Main(string[] args)
        {
            var scrapeUrl = "/scraper/scrape";
            var jobs = new List<Job>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(localIISUrl);

                Parallel.ForEach(GetUrls(), (url) =>
                {
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("url", url),
                        new KeyValuePair<string, string>("selectors[]", "h1"),
                        new KeyValuePair<string, string>("selectors[]", "h2"),
                        new KeyValuePair<string, string>("selectors[]", "h3")
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

            Console.WriteLine();
            Console.WriteLine("Enter to exit.");

            Console.ReadLine();
        }

        static IEnumerable<string> GetUrls()
        {
            var urlListPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UrlList.txt");

            if (!File.Exists(urlListPath))
                yield return localIISUrl;

            foreach(var line in File.ReadAllLines(urlListPath))
                yield return line;
        }
    }
}
