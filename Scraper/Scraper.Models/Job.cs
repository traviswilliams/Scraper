using System;

namespace Scraper.Models
{
    public class Job : IJob
    {
        public Guid Id { get; private set; }

        public string Url { get; set; }

        public string Result { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Pending;

        public Job() : this(Guid.NewGuid()) { }

        public Job(Guid id) : this(id, string.Empty) { }

        public Job(string url) : this(Guid.NewGuid(),  url) { }

        public Job(Guid id, string url)
        {
            Id = id;
            Url = url;
        }

        public override string ToString()
        {
            return $"Job: {Id} -- [{Status}]:  {Url}";
        }

        public IJob Clone()
        {
            return new Job
            {
                Url = Url,
                Status = Status
            };
        }
    }
}
