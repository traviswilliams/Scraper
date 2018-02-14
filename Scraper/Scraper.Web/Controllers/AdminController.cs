using System.Web.Http;
using Scraper.Services;

namespace Scraper.Web.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : BaseController
    {
        public AdminController(IJobManager scraperManager) : base(scraperManager) { }

        /// <summary>
        /// Get the current status of the scraper.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("status")]
        public IHttpActionResult Status()
        {
            return Ok(ScraperManager.Status.ToString());
        }

        /// <summary>
        /// Start the scraping job.
        /// </summary>
        [HttpPost, Route("start")]
        public void Start()
        {
            ScraperManager.Start();
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
        /// Update the concurrency
        /// </summary>
        /// <param name="maxConcurrency">Maximum number of scrapers allowed to run concurrently</param>
        [HttpPost, Route("concurrency/{maxConcurrency}")]
        public IHttpActionResult UpdateConcurrency(int maxConcurrency)
        {
            const int max = 100;

            if (maxConcurrency < 1 || maxConcurrency > max)
                return BadRequest($"maxConcurrency needs to be a positive number less than {max}.");

            ScraperManager.MaxScrapers = maxConcurrency;

            return Ok();
        }

        /// <summary>
        /// Update the concurrency
        /// </summary>
        /// <param name="maxConcurrency">Maximum number of scrapers allowed to run concurrently</param>
        [HttpGet, Route("concurrency")]
        public IHttpActionResult GetConcurrency()
        {
            return Ok(ScraperManager.MaxScrapers);
        }
    }
}