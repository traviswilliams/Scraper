using System.Collections.Generic;

namespace Scraper.Models
{
    interface IJobManager
    {
        int CurrentlyRunningJobs();

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
