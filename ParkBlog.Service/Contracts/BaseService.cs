using Kendo.DynamicLinq;
using ParkBlog.Common.Extensions;
using ParkBlog.Data.Contracts;
using ParkBlog.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace ParkBlog.Service.Contracts
{
    /// <summary>
    /// Base service for common crud operations. this is a replacement for repository pattern.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseService<TEntity> : IDisposable where TEntity : BaseEntity
    {
        private readonly IQueryableContext _queryableContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{TEntity}"/> class.
        /// </summary>
        /// <param name="queryableContext">The queryable context.</param>
        protected BaseService(IQueryableContext queryableContext)
        {
            _queryableContext = queryableContext;
        }

        /// <summary>
        /// Adds the new item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddItem(TEntity item)
        {
            if (item == null) return;
            EntitySet().Add(item);
        }

        /// <summary>
        /// Edits the item.
        /// </summary>
        /// <param name="updatedItem">The updated item.</param>
        public void EditItem(TEntity updatedItem)
        {
            if (updatedItem == null) return;
            _queryableContext.SetModified(updatedItem);
        }

        /// <summary>
        /// Edits the item.
        /// </summary>
        /// <param name="originalItem">The original item.</param>
        /// <param name="updatedItem">The updated item.</param>
        public void EditItem(TEntity originalItem, TEntity updatedItem)
        {
            _queryableContext.ApplyCurrentValues(originalItem, updatedItem);
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void DeleteItem(TEntity item)
        {
            if (item == null) return;
            _queryableContext.Attach(item);
            EntitySet().Remove(item);
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        public void DeleteItem(Expression<Func<TEntity, bool>> predicate)
        {
            var item = EntitySet().Single(predicate);
            if (item != null)
                EntitySet().Remove(item);
        }

        /// <summary>
        /// Finds the item by using ef find method.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>`0.</returns>
        public TEntity FindItem(params object[] keys)
        {
            //Declare 'foundedItem' and set it by calling 'Find' method of DbSet
            var foundedItem = EntitySet().Find(keys);

            //Return 'foundedItem'
            return (foundedItem);
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>IQueryable{`0}.</returns>
        public IQueryable<TEntity> GetItems(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = predicate == null ? EntitySet() : EntitySet().Where(predicate);
            if (includeProperties != null)
                query = ApplyIncludesOnQuery(query, includeProperties);

            return query;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>IQueryable{`0}.</returns>
        public IQueryable<TEntity> GetItems(Filter predicate, int take, int skip, SortOrder sortOrder, string sortColumn,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = predicate != null ? EntitySet().FilterData(predicate) : EntitySet();
            if (includeProperties != null)
                query = ApplyIncludesOnQuery(query, includeProperties);
            query = query.SortBy(sortOrder, sortColumn).Skip(skip).Take(take);

            return query;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>`0.</returns>
        public TEntity GetItem(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = EntitySet().AsQueryable();
            if (includeProperties != null)
                query = ApplyIncludesOnQuery(query, includeProperties);

            return query.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Gets the first item.
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>`0.</returns>
        public TEntity GetFirstItem(Expression<Func<TEntity, int>> sort, Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? EntitySet().OrderByDescending(sort).FirstOrDefault()
                       : EntitySet().Where(predicate).OrderBy(sort).FirstOrDefault();
        }

        /// <summary>
        /// Gets the last item.
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>`0.</returns>
        public TEntity GetLastItem(Expression<Func<TEntity, int>> sort, Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                       ? EntitySet().OrderByDescending(sort).FirstOrDefault()
                       : EntitySet().Where(predicate).OrderByDescending(sort).FirstOrDefault();
        }

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate == null ? EntitySet().Count() : EntitySet().Where(predicate).Count();
        }

        /// <summary>
        /// Anies the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate == null ? EntitySet().Any() : EntitySet().Any(predicate);
        }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        public void Commit()
        {
            _queryableContext.Commit();
        }

        /// <summary>
        /// Commits the and refresh changes.
        /// </summary>
        public void CommitAndRefreshChanges()
        {
            _queryableContext.CommitAndRefreshChanges();
        }

        /// <summary>
        /// Rollbacks the changes.
        /// </summary>
        public void RollbackChanges()
        {
            _queryableContext.RollbackChanges();
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IEnumerable{`0}.</returns>
        public IEnumerable<TEntity> ExecuteQuery(string sqlQuery, params object[] parameters)
        {
            return _queryableContext.ExecuteQuery<TEntity>(sqlQuery, parameters);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteCommand(string sqlQuery, params object[] parameters)
        {
            return _queryableContext.ExecuteCommand(sqlQuery, parameters);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (_queryableContext == null) return;
            _queryableContext.Dispose();
        }

        #region Private methods

        /// <summary>
        /// Returns Dbset of an entity.
        /// </summary>
        /// <returns>IDbSet.</returns>
        private IDbSet<TEntity> EntitySet()
        {
            return _queryableContext.EntitySet<TEntity>();
        }

        /// <summary>
        /// Applies the includes on query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>IQueryable{`0}.</returns>
        private static IQueryable<TEntity> ApplyIncludesOnQuery(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //Return Applied Includes query
            return (includeProperties.Aggregate(query, (current, include) => current.Include(include)));
        }

        #endregion Private methods
    }
}