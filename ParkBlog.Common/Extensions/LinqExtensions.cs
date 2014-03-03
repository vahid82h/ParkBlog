using Kendo.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace ParkBlog.Common.Extensions
{
    /// <summary>
    /// Class LinqExtensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Sorts the by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns>IOrderedQueryable{``0}.</returns>
        public static IOrderedQueryable<T> SortBy<T>(this IQueryable<T> source, SortOrder sortOrder, string sortColumn)
        {
            //var param = Expression.Parameter(typeof(T), "p");
            var prop = typeof(T).GetProperty(sortColumn);
            var propType = prop.PropertyType;

            var props = new List<string> { sortColumn };
            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                props.Add("Value");

            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var pi in props.Select(property => type.GetProperty(property)))
            {
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var methodName = sortOrder == SortOrder.Ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// Filters the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IQueryable{``0}.</returns>
        public static IQueryable<T> FilterData<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort,
            Filter filter)
        {
            queryable = Filter(queryable, filter);
            queryable = Sort(queryable, sort);
            queryable = Page(queryable, take, skip);
            return queryable;
        }

        /// <summary>
        /// Filters the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IQueryable{``0}.</returns>
        public static IQueryable<T> FilterData<T>(this IQueryable<T> queryable, Filter filter)
        {
            queryable = Filter(queryable, filter);
            return queryable;
        }

        #region Private methods

        /// <summary>
        /// Filters the specified queryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IQueryable{``0}.</returns>
        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
        {
            if (filter == null || filter.Logic == null)
                return queryable;

            var filters = filter.All();
            var objArray = filters.Select(f => f.Value).ToArray();
            objArray = ConvertData(objArray);
            var predicate = filter.ToExpression(filters);
            queryable = queryable.Where(predicate, objArray);
            return queryable;
        }

        /// <summary>
        /// Sorts the specified queryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="sort">The sort.</param>
        /// <returns>IQueryable{``0}.</returns>
        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort == null || !sort.Any())
                return queryable;
            var ordering = string.Join(",", sort.Select(s => s.ToExpression()));
            return queryable.OrderBy(ordering, new object[0]);
        }

        /// <summary>
        /// Pages the specified queryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">The queryable.</param>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <returns>IQueryable{``0}.</returns>
        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return Queryable.Take(Queryable.Skip(queryable, skip), take);
        }

        /// <summary>
        /// Applies the order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="property">The property.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>IOrderedQueryable{``0}.</returns>
        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            var props = property.Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// Converts the string data to actual types.
        /// </summary>
        /// <param name="objArray">The object array.</param>
        /// <returns>System.Object[][].</returns>
        private static object[] ConvertData(object[] objArray)
        {
            for (var i = 0; i < objArray.Length; i++)
            {
                if (objArray[i].ToString().Equals("true"))
                {
                    objArray[i] = true;
                    continue;
                }
                if (objArray[i].ToString().Equals("false"))
                {
                    objArray[i] = false;
                    continue;
                }

                int intNumber;
                var isIntNumber = Int32.TryParse(objArray[i].ToString(), out intNumber);
                if (isIntNumber)
                {
                    objArray[i] = intNumber;
                    continue;
                }

                double doubleNumber;
                var isDoubleNumber = Double.TryParse(objArray[i].ToString(), out doubleNumber);
                if (isDoubleNumber)
                    objArray[i] = doubleNumber;
            }

            return objArray;
        }

        #endregion Private methods
    }
}