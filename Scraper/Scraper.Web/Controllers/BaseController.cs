using Scraper.Services;
using System.Web.Http;

namespace Scraper.Web.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected IJobManager ScraperManager { get; }

        public BaseController(IJobManager scraperManager)
        {
            ScraperManager = scraperManager;
        }
    }
}