using Scraper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Scraper.DataAccess
{
    public class InMemoryRepository : IRepository<Job>
    {
        public void Save(Job item)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            DataStore.Instance.Store.AddOrUpdate(item.Id, item, (key, old) => item);
        }

        public void Delete(Job item)
        {
            Delete(item.Id);
        }

        public void Delete(Guid id)
        {
            DataStore.Instance.Store.TryRemove(id, out var oldJob);
        }

        public Job Get(Guid id)
        {
            DataStore.Instance.Store.TryGetValue(id, out var job);
            return job;
        }

        public IEnumerable<Job> GetByStatus(JobStatus status, int numResults = 25, int pageNumber = 1)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            if (numResults < 1 || numResults > 50)
                numResults = 25;

            return DataStore.Instance.Store
                .Where(i => i.Value.Status == status)
                .Select(i => i.Value)
                .Skip((pageNumber -1) * numResults)
                .Take(numResults)
                .ToList();
        }

        public IEnumerable<Job> Where(Predicate<Job> query, int numResults = 25, int pageNumber = 1)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            if (numResults < 1 || numResults > 50)
                numResults = 25;

            return DataStore.Instance.Store
                .Where(i => query(i.Value))
                .Select(i => i.Value)
                .Skip((pageNumber - 1) * numResults)
                .Take(numResults)
                .ToList();
        }

        /// <summary>
        /// Singleton to store data in memory
        /// </summary>
        private class DataStore
        {
            private static object lockObject = new object();

            private static DataStore _instance;
            internal static DataStore Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (lockObject)
                        {
                            if (_instance == null)
                            {
                                _instance = new DataStore();
                            }
                        }
                    }

                    return _instance;
                }
            }

            internal ConcurrentDictionary<Guid, Job> Store { get; private set; }

            private DataStore()
            {
                /* Disallow creation of this class outside of the class itself. */
                Store = new ConcurrentDictionary<Guid, Job>();
            }
        }
    }
}
