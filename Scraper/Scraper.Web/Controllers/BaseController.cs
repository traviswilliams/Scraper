using Scraper.DataAccess;
using Scraper.Models;
using Scraper.Services;
using System.Web.Http;

namespace Scraper.Web.Controllers
{
    /// <summary>
    /// Provide base functionality to different controllers.
    /// </summary>
    public abstract class BaseController : ApiController
    {
        /// <summary>
        /// Job Manager
        /// </summary>
        protected IJobManager JobManager { get; }

        /// <summary>
        /// Job Repository
        /// </summary>
        protected IRepository<Job> JobRepository { get; }

        /// <summary>
        /// Create an Instance of the controller.
        /// </summary>
        public BaseController(IJobManager jobManager, IRepository<Job> jobRepository)
        {
            JobManager = jobManager;
            JobRepository = jobRepository;
        }
    }
} 