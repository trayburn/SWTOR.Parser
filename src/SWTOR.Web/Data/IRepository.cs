using System;
using System.Collections.Generic;
using System.Linq;

namespace SWTOR.Web.Data
{
    public interface IRepository<T>
            where T : class
    {
        IQueryable<T> Query();
        T Store(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
