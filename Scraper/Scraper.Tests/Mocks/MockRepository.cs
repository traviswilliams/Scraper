using Scraper.DataAccess;
using Scraper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Scraper.Tests.Mocks
{
    internal class MockRepository : IRepository<Job>
    {
        internal Dictionary<Guid, Job> Items = new Dictionary<Guid, Job>();

        public void Delete(Job item)
        {
            Delete(item.Id);
        }

        public void Delete(Guid id)
        {
            if (Items.ContainsKey(id))
                Items.Remove(id);
        }

        public Job Get(Guid id)
        {
            Items.TryGetValue(id, out var item);
            return item;
        }

        public IEnumerable<Job> GetByStatus(JobStatus status, int numResults = 25, int pageNumber = 1)
        {
            return Items.Values.Where(i => i.Status == status)
                .Skip((pageNumber - 1) * numResults)
                .Take(numResults);
        }

        public void Save(Job item)
        {
            if (Items.ContainsKey(item.Id))
                Items[item.Id] = item;
            else
                Items.Add(item.Id, item);
        }

        public IEnumerable<Job> Where(Predicate<Job> query, int numResults = 25, int pageNumber = 1)
        {
            return Items.Values.Where(i => query(i))
                .Skip((pageNumber - 1) * numResults)
                .Take(numResults);
        }
    }
}
