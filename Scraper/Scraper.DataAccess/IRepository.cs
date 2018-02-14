using Scraper.Models;
using System;
using System.Collections.Generic;

namespace Scraper.DataAccess
{
    public interface IRepository<T>
    {
        void Save(T item);

        void Delete(T item);

        void Delete(Guid id);

        T Get(Guid id);

        IEnumerable<T> GetByStatus(JobStatus status);

        IEnumerable<T> Where(Predicate<T> query);
    }
}
