using System;

namespace Scraper.Models
{
    public class Job
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Result { get; set; }

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
