using Scraper.Models;
using Scraper.Services.Models;
using System;
using System.Collections.Generic;

namespace Scraper.Services
{
    public interface IJobManager
    {
        int MaxScrapers { get; set; }

        int CurrentlyRunningJobs { get; }

        ManagerStatus Status { get; }

        void Start();

        void Stop();

        void Pause();

        void Resume();

        void QueueJob(IJob job);

        /// <summary>
        /// Get a job.
        /// </summary>
        /// <returns></returns>
        IJob GetJob(Guid id);

        /// <summary>
        /// Get all jobs with a particular status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IEnumerable<IJob> GetJobs(JobStatus status);
    }
}
