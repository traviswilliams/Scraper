﻿using Scraper.Models;
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

        IEnumerable<T> GetByStatus(JobStatus status, int numResults = 25, int pageNumber = 1);

        IEnumerable<T> Where(Predicate<T> query, int numResults = 25, int pageNumber = 1);
    }
}
