using Scraper.Models;
using Scraper.Services;
using Scraper.Web.Models;
using System;
using System.Web.Http;

namespace Scraper.Web.Controllers
{
    [RoutePrefix("scraper")]
    public class ScraperController : BaseController
    {
        public ScraperController(IJobManager scraperManager) : base(scraperManager) {  }

        /// <summary>
        /// Add a scrape job
        /// </summary>
        /// <param name="url">The url of the site to scrape.</param>
        [HttpPost, Route("scrape")]
        public IHttpActionResult Scrape(ScrapeRequest request)
        {
            var job = new Job(request.Url);
            JobManager.QueueJob(job);

            return Ok(job);
        }

        [HttpGet, Route("job/{id}")]
        public IHttpActionResult GetJob(Guid id)
        {
            var job = JobManager.GetJob(id);

            return job == null
                ? (IHttpActionResult)NotFound()
                : Ok(job);
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("pending")]
        public IHttpActionResult GetPendingJobs()
        {
            return Ok(JobManager.GetJobs(JobStatus.Pending));
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("running")]
        public IHttpActionResult GetRunningJobs()
        {
            return Ok(JobManager.GetJobs(JobStatus.Running));
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("completed")]
        public IHttpActionResult GetCompleted()
        {
            return Ok(JobManager.GetJobs(JobStatus.Completed));
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("failed")]
        public IHttpActionResult GetFailedJobs()
        {
            return Ok(JobManager.GetJobs(JobStatus.Failed));
        }
    }
}
