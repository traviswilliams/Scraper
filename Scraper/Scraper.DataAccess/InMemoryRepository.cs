using Scraper.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Scraper.DataAccess
{
    public class InMemoryRepository : IRepository<IJob>
    {
        public void Save(IJob item)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            DataStore.Instance.Store.AddOrUpdate(item.Id, item, (key, old) => item);
        }

        public void Delete(IJob item)
        {
            Delete(item.Id);
        }

        public void Delete(Guid id)
        {
            DataStore.Instance.Store.TryRemove(id, out var oldJob);
        }

        public IJob Get(Guid id)
        {
            DataStore.Instance.Store.TryGetValue(id, out var job);
            return job;
        }

        public IEnumerable<IJob> GetByStatus(JobStatus status)
        {
            return DataStore.Instance.Store
                .Where(i => i.Value.Status == status)
                .Select(i => i.Value)
                .ToList();
        }

        public IEnumerable<IJob> Where(Predicate<IJob> query)
        {
            return DataStore.Instance.Store
                .Where(i => query(i.Value))
                .Select(i => i.Value)
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

            internal ConcurrentDictionary<Guid, IJob> Store { get; private set; }

            private DataStore()
            {
                /* Disallow creation of this class outside of the class itself. */
                Store = new ConcurrentDictionary<Guid, IJob>();
            }
        }
    }
}
