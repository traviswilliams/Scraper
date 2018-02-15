using System;
using System.Collections.Generic;

namespace Scraper.Models
{
    public class Job
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public IEnumerable<string> Selectors { get; set; } = new List<string>();

        public IDictionary<string, IEnumerable<string>> Result { get; set; } = new Dictionary<string, IEnumerable<string>>();

        public JobStatus Status { get; set; } = JobStatus.Pending;

        public Job() { }

        public Job(Guid id) 
        {
            Id = id;
        }

        public Job(string url)
        {
            Url = url;
        }

        public Job(Guid id, string url)
        {
            Id = id;
            Url = url;
        }

        public override string ToString()
        {
            return $"Job: {Id} -- [{Status}]:  {Url}";
        }
    }
}
