using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper.Services;
using Scraper.Tests.Mocks;
using Scraper.Services.Models;
using Scraper.DataAccess;
using Scraper.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Scraper.Tests
{
    [TestClass]
    public class JobManagerTests
    {
        private IRepository<Job> JobRepository { get; set; }
        private IJobManager JobManager { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            JobRepository = new MockRepository();
            JobManager = new JobManager(new MockScraper(), JobRepository);
        }

        [TestMethod]
        public void JobManager_CanStart()
        {
            JobManager.Start();
            Assert.AreEqual(ManagerStatus.Started, JobManager.Status);
        }

        [TestMethod]
        public void JobManager_CanStop()
        {
            JobManager.Stop();
            Assert.AreEqual(ManagerStatus.Stopped, JobManager.Status);
        }

        [TestMethod]
        public void JobManager_CanPauseAndResume()
        {
            JobManager.Pause();
            Assert.AreEqual(ManagerStatus.Paused, JobManager.Status);
            JobManager.Resume();
            Assert.AreEqual(ManagerStatus.Started, JobManager.Status);
        }

        [TestMethod]
        public void JobManager_JobHitsDifferentStates()
        {
            var currentPending = JobRepository.GetByStatus(JobStatus.Pending).Count();
            var currentComplete = JobRepository.GetByStatus(JobStatus.Completed).Count();

            JobManager.Pause();
            JobManager.QueueJob(new Models.Job("www.sometest.com"));

            Assert.AreEqual(currentPending + 1, JobRepository.GetByStatus(JobStatus.Pending).Count());

            JobManager.Resume();

            //Wait a second.
            Task.Delay(TimeSpan.FromSeconds(.5)).Wait();

            Assert.AreEqual(currentComplete + 1, JobRepository.GetByStatus(JobStatus.Completed).Count());
        }
    }
}
