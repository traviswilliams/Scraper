using Scraper.Services;
using System.Web.Http;

namespace Scraper.Web.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected IJobManager JobManager { get; }

        public BaseController(IJobManager scraperManager)
        {
            JobManager = scraperManager;
        }
    }
} 