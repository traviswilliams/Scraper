using Scraper.DataAccess;
using Scraper.Models;
using Scraper.Services;
using Scraper.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Scraper.Web.Controllers
{
    /// <summary>
    /// Expose functionality to scrape web pages.
    /// </summary>
    [RoutePrefix("scraper")]
    public class ScraperController : BaseController
    {
        /// <summary>
        /// Create a new instance of the ScraperController
        /// </summary>
        /// <param name="jobManager"></param>
        /// <param name="jobRepository"></param>
        public ScraperController(IJobManager jobManager, IRepository<Job> jobRepository) : base(jobManager, jobRepository) {  }

        /// <summary>
        /// Add a scrape job
        /// </summary>
        [HttpPost, Route("scrape")]
        [ResponseType(typeof(Job))]
        public IHttpActionResult Scrape(ScrapeRequest request)
        {
            //If we've already scraped the same url, re-use the same job.
            var existingJob = JobRepository
                .Where((i) => i.Url.ToLowerInvariant() == request.Url.ToLowerInvariant())
                .FirstOrDefault();

            var job = existingJob ?? new Job(request.Url);
            job.Selectors = request.Selectors;

            JobManager.QueueJob(job);

            return Ok(job);
        }

        /// <summary>
        /// Get a job by id.
        /// </summary>
        [HttpGet, Route("job/{id}")]
        [ResponseType(typeof(Job))]
        public IHttpActionResult GetJob(Guid id)
        {
            var job = JobRepository.Get(id);

            return job == null
                ? (IHttpActionResult)NotFound()
                : Ok(job);
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("pending")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetPendingJobs(int numResults = 5, int pageNumber = 1)
        {
            return Ok(JobRepository.GetByStatus(JobStatus.Pending, numResults, pageNumber));
        }

        /// <summary>
        /// Get a list of running jobs
        /// </summary>
        [HttpGet, Route("running")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetRunningJobs(int numResults = 5, int pageNumber = 1)
        {
            return Ok(JobRepository.GetByStatus(JobStatus.Running, numResults, pageNumber));
        }

        /// <summary>
        /// Get a list of completed jobs
        /// </summary>
        [HttpGet, Route("completed")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetCompleted(int numResults = 5, int pageNumber = 1)
        {
            return Ok(JobRepository.GetByStatus(JobStatus.Completed, numResults, pageNumber));
        }

        /// <summary>
        /// Get a list of failed jobs
        /// </summary>
        [HttpGet, Route("failed")]
        [ResponseType(typeof(IEnumerable<Job>))]
        public IHttpActionResult GetFailedJobs(int numResults = 5, int pageNumber = 1)
        {
            return Ok(JobRepository.GetByStatus(JobStatus.Failed, numResults, pageNumber));
        }
    }
}
