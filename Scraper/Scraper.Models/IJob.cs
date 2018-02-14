using System;

namespace Scraper.Models
{
    public interface IJob
    {
        Guid Id { get; set; }

        string Url { get; set; }

        JobStatus Status { get; set; }

        string Result { get; set; }
    }
}
