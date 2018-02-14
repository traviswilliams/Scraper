using Scraper.Models;
using System.Collections.Generic;

namespace Scraper.Services
{
    public interface IJobManager
    {
        int MaxScrapers { get; set; }

        int CurrentlyRunningJobs { get; }

        void Start();

        void Stop();

        void Pause();

        void Resume();

        void QueueJob(IJob job);

        /// <summary>
        /// Get all jobs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IJob> GetJobs();

        /// <summary>
        /// Get all jobs with a particular status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IEnumerable<IJob> GetJobs(JobStatus status);
    }
}
