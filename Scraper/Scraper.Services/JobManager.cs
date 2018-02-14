using Scraper.DataAccess;
using Scraper.Models;
using Scraper.Services.Extensions;
using Scraper.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper.Services
{
    public class JobManager : IJobManager
    {
        private IScraperService Scraper { get; }
        private IRepository<IJob> JobRepository { get; }

        private int maxScrapers = 2;

        public int MaxScrapers
        {
            get { return maxScrapers; }
            set
            {
                maxScrapers = value < 1 
                    ? 1 
                    : value;
            }
        }

        private volatile ManagerStatus _status = ManagerStatus.Stopped;
        public ManagerStatus Status => _status;

        public int CurrentlyRunningJobs => throw new NotImplementedException();

        private ConcurrentQueue<IJob> PendingJobQueue { get; } = new ConcurrentQueue<IJob>();
        private ConcurrentDictionary<IJob, Task<ScrapeResult>> RunningJobs { get; } = new ConcurrentDictionary<IJob, Task<ScrapeResult>>();

        private Thread jobRunner;
        private static object lockObject = new object();
        private static EventWaitHandle waitHandle = new ManualResetEvent(true);

        public JobManager(IScraperService scraper, IRepository<IJob> repository)
        {
            Scraper = scraper;
            JobRepository = repository;

            Start();
        }

        /// <summary>
        /// Kick off a long running thread which processes scrape jobs.
        /// </summary>
        public void Start()
        {
            if (jobRunner != null)
                return; 

            jobRunner = new Thread(new ThreadStart(() =>
            {
                _status = ManagerStatus.Started;

                while (Status != ManagerStatus.Stopped)
                {
                    ProcessWaitingJobs();

                    ProcessRunningJobs();

                    waitHandle.WaitOne();
                }
            }));

            jobRunner.Start();
        }

        public void Stop()
        {
            if (jobRunner != null)
            {
                _status = ManagerStatus.Stopped;
                jobRunner = null;
            }
        }

        public void Pause()
        {
            if (jobRunner == null)
                return;

            _status = ManagerStatus.Paused;

            waitHandle.Reset();
        }

        public void Resume()
        {
            if (jobRunner == null)
                return;

            _status = ManagerStatus.Started;

            waitHandle.Set();
        }

        public void QueueJob(IJob job)
        {
            JobRepository.Save(job);
            PendingJobQueue.Enqueue(job);
        }

        public IJob GetJob(Guid id)
        {
            return JobRepository.Get(id);
        }

        public IEnumerable<IJob> GetJobs(JobStatus status)
        {
            return JobRepository.GetByStatus(status);
        }

        /// <summary>
        /// Start any pending jobs as soon as we have capacity.
        /// </summary>
        private void ProcessWaitingJobs()
        {
            IJob pendingJob = null;

            if (CanProcessJob())
            {
                lock (lockObject)
                {
                    if (CanProcessJob())
                    {
                        PendingJobQueue.TryDequeue(out pendingJob);
                        pendingJob.Status = JobStatus.Running;

                        JobRepository.Save(pendingJob);
                        RunningJobs.TryAdd(pendingJob, Scraper.ScrapeAsync(pendingJob.Url));
                    }
                }
            }
        }

        /// <summary>
        /// Process running jobs
        /// </summary>
        private void ProcessRunningJobs()
        {
            foreach (var runningJob in RunningJobs.Keys)
            {
                ProcessRunningJob(runningJob);
            }
        }

        /// <summary>
        /// Remove jobs when they succeed/fail
        /// </summary>
        /// <param name="job"></param>
        private void ProcessRunningJob(IJob job)
        {
            if (!RunningJobs.TryGetValue(job, out var scrapeResult))
                return;

            lock (lockObject)
            {
                if (scrapeResult.IsCompleted)
                {
                    var result = RunningJobs[job].Result;

                    job.Status = result.Error != null ? JobStatus.Failed : JobStatus.Completed;
                    job.Result = result.Error != null ? result.Error.GetFullExceptionMessage() : result.Body;

                    JobRepository.Save(job);
                    RunningJobs.TryRemove(job, out var removedJob);
                }
                else if (scrapeResult.IsFaulted)
                {
                    job.Status = JobStatus.Failed;
                    job.Result = scrapeResult.Exception.GetFullExceptionMessage();

                    JobRepository.Save(job);
                    RunningJobs.TryRemove(job, out var removedJob);
                }
            }
        }

        private bool CanProcessJob()
        {
            return PendingJobQueue.Any() && RunningJobs.Count < MaxScrapers;
        }
    }
}
