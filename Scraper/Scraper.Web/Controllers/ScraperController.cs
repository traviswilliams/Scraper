using Scraper.Models;
using Scraper.Services;
using System.Web.Http;

namespace Scraper.Web.Controllers
{
    [RoutePrefix("scraper")]
    public class ScraperController : ApiController
    {
        private IJobManager ScraperManager { get; }

        public ScraperController(IJobManager scraperManager)
        {
            ScraperManager = scraperManager;
        }

        /// <summary>
        /// Start the scraping job.
        /// </summary>
        [HttpPost, Route("start")]
        public void Start()
        {
            ScraperManager.Stop();
        }

        /// <summary>
        /// Stop the scraper. This forces scraping to stop and will fail any currently running jobs.
        /// </summary>
        [HttpPost, Route("stop")]
        public void Stop()
        {
            ScraperManager.Stop();
        }

        /// <summary>
        /// Pause the scraper.
        /// </summary>
        [HttpPost, Route("pause")]
        public void Pause()
        {
            ScraperManager.Pause();
        }

        /// <summary>
        /// Resume the scraper.
        /// </summary>
        [HttpPost, Route("resume")]
        public void Resume()
        {
            ScraperManager.Resume();
        }

        /// <summary>
        /// Add a scrape job
        /// </summary>
        /// <param name="url">The url of the site to scrape.</param>
        [HttpGet, Route("scrape/{url}")]
        public IHttpActionResult Scrape(string url)
        {
            var job = new Job(url);
            ScraperManager.QueueJob(job);

            return Ok(job);
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("pending")]
        public IHttpActionResult GetPendingJobs()
        {
            return Ok(ScraperManager.GetJobs(JobStatus.Pending));
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("running")]
        public IHttpActionResult GetRunningJobs()
        {
            return Ok(ScraperManager.GetJobs(JobStatus.Pending));
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("completed")]
        public IHttpActionResult GetCompleted()
        {
            return Ok(ScraperManager.GetJobs(JobStatus.Pending));
        }

        /// <summary>
        /// Get a list of pending jobs
        /// </summary>
        [HttpGet, Route("failed")]
        public IHttpActionResult GetFailedJobs()
        {
            return Ok(ScraperManager.GetJobs(JobStatus.Pending));
        }

    }
}
