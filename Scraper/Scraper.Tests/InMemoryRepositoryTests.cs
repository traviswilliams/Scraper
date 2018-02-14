using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper.Models;
using Scraper.DataAccess;
using System.Linq;

namespace Scraper.Tests
{
    [TestClass]
    public class InMemoryRepositoryTests
    {
        private IRepository<Job> JobRepository { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            JobRepository = new InMemoryRepository();
        }

        [TestMethod]
        public void JobRepository_CanAdd()
        {
            var job = new Job(Guid.NewGuid(), "test.com");
            JobRepository.Save(job);
            Assert.IsNotNull(JobRepository.Get(job.Id));
        }

        [TestMethod]
        public void JobRepository_CanDelete()
        {
            var job = new Job(Guid.NewGuid(), "test.com");
            JobRepository.Save(job);
            Assert.IsNotNull(JobRepository.Get(job.Id));
            JobRepository.Delete(job);
            Assert.IsNull(JobRepository.Get(job.Id));
        }

        public void JobRepository_CanGetByStatus()
        {
            var job = new Job(Guid.NewGuid(), "test.com") { Status = JobStatus.Pending };
            JobRepository.Save(job);

            Assert.AreEqual(1, JobRepository.GetByStatus(JobStatus.Pending).Count());

            job.Status = JobStatus.Completed;
            JobRepository.Save(job);

            Assert.AreEqual(1, JobRepository.GetByStatus(JobStatus.Completed).Count());
        }
    }
}
