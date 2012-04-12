using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Castle.Core.Logging;

namespace SWTOR.Web.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private IDocumentSession session;
        public ILogger Logger;

        public Repository(IDocumentSession session)
        {
            this.session = session;
            this.Logger = NullLogger.Instance;
        }

        public IQueryable<T> Query()
        {
            return session.Query<T>();
        }

        public T Store(T entity)
        {
            session.Store(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            session.Delete(entity);
        }

        public void SaveChanges()
        {
            session.SaveChanges();
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}