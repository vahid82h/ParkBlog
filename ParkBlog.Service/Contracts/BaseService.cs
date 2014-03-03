using ParkBlog.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ParkBlog.Service.Contracts
{
    public abstract class BaseService<T> : IDisposable where T : class
    {
        private readonly IQueryableContext _queryableContext;

        protected BaseService(IQueryableContext queryableContext)
        {
            _queryableContext = queryableContext;
        }

        public void AddItem(T item)
        {
            if (item == null) return;
            EntitySet().Add(item);
        }

        public void EditItem(T updatedItem, int id)
        {
            if (updatedItem == null) return;
            _queryableContext.SetModified(updatedItem, id);
        }

        public void EditItem(T originalItem, T updatedItem)
        {
            _queryableContext.ApplyCurrentValues(originalItem, updatedItem);
        }

        public void DeleteItem(T item)
        {
            if (item == null) return;
            _queryableContext.Attach(item);
            EntitySet().Remove(item);
        }

        public void DeleteItem(Expression<Func<T, bool>> predicate)
        {
            var item = EntitySet().Single(predicate);
            if (item != null)
                EntitySet().Remove(item);
        }

        public T FindItem(params object[] keys)
        {
            //Declare 'foundedItem' and set it by calling 'Find' method of DbSet
            var foundedItem = EntitySet().Find(keys);

            //Return 'foundedItem'
            return (foundedItem);
        }

        public IQueryable<T> GetItems(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = predicate == null ? EntitySet() : EntitySet().Where(predicate);
            if (includeProperties != null)
                query = ApplyIncludesOnQuery(query, includeProperties);

            return query;
        }

        //public IQueryable<T> GetItems(Filter predicate, int take, int skip, SortOrder sortOrder, string sortColumn,
        //    params Expression<Func<T, object>>[] includeProperties)
        //{
        //    var query = predicate != null ? EntitySet().FilterData(predicate) : EntitySet();
        //    if (includeProperties != null)
        //        query = ApplyIncludesOnQuery(query, includeProperties);
        //    query = query.SortBy(sortOrder, sortColumn).Skip(skip).Take(take);

        //    return query;
        //}

        public T GetItem(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = EntitySet().AsQueryable();
            if (includeProperties != null)
                query = ApplyIncludesOnQuery(query, includeProperties);

            return query.SingleOrDefault(predicate);
        }

        public T GetFirstItem(Expression<Func<T, int>> sort, Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                       ? EntitySet().OrderByDescending(sort).FirstOrDefault()
                       : EntitySet().Where(predicate).OrderBy(sort).FirstOrDefault();
        }

        public T GetLastItem(Expression<Func<T, int>> sort, Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                       ? EntitySet().OrderByDescending(sort).FirstOrDefault()
                       : EntitySet().Where(predicate).OrderByDescending(sort).FirstOrDefault();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? EntitySet().Count() : EntitySet().Where(predicate).Count();
        }

        //public int Count(Filter predicate)
        //{
        //    return predicate == null ? EntitySet().Count() : EntitySet().FilterData(predicate).Count();
        //}

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? EntitySet().Any() : EntitySet().Any(predicate);
        }

        public void CommitAndRefereshChanges()
        {
            _queryableContext.CommitAndRefreshChanges();
        }

        public void RollbackChnages()
        {
            _queryableContext.RollbackChanges();
        }

        public IEnumerable<T> ExecuteQuery(string sqlQuery, params object[] parameters)
        {
            return _queryableContext.ExecuteQuery<T>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlQuery, params object[] parameters)
        {
            return _queryableContext.ExecuteCommand(sqlQuery, parameters);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        #region Private methods

        private IDbSet<T> EntitySet()
        {
            return _queryableContext.Set<T>();
        }

        private static IQueryable<T> ApplyIncludesOnQuery(IQueryable<T> query, params Expression<Func<T, object>>[] includeProperties)
        {
            //Return Applied Includes query
            return (includeProperties.Aggregate(query, (current, include) => current.Include(include)));
        }

        private void Dispose()
        {
            if (_queryableContext == null) return;
            _queryableContext.Dispose();
        }

        #endregion Private methods
    }
}