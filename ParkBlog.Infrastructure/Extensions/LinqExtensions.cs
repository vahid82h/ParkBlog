using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace ParkBlog.Infrastructure.Extensions
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy2<TSource>(this IQueryable<TSource> source,
            SortOrder sortOrder,
            string sortColumn)
        {
            var param = Expression.Parameter(typeof(TSource), "p");
            var prop = typeof(TSource).GetProperty(sortColumn);
            var propType = prop.PropertyType;
            //var sortExpression = Expression.Lambda<Func<TSource, object>>(Expression.Property(param, sortColumn), param);
            //var sortExpression =
            //    Expression.Lambda<Func<TSource, object>>(Expression.Convert(Expression.Property(param, sortColumn),
            //                                                                typeof (object)), param);
            if (propType == typeof(DateTime))
            {
                var sortExpression = Expression.Lambda<Func<TSource, DateTime>>(Expression.Property(param, sortColumn),
                    param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }
            if (propType == typeof(double))
            {
                var sortExpression = Expression.Lambda<Func<TSource, double>>(Expression.Property(param, sortColumn),
                    param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }

            if (propType == typeof(int))
            {
                var sortExpression = Expression.Lambda<Func<TSource, int>>(Expression.Property(param, sortColumn), param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }
            if (propType == typeof(string))
            {
                var sortExpression = Expression.Lambda<Func<TSource, string>>(Expression.Property(param, sortColumn),
                    param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }

            throw new NotSupportedException("Object type resolution not implemented for this type");
        }

        public static IOrderedQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> source,
            SortOrder sortOrder,
            string sortColumn)
        {
            var param = Expression.Parameter(typeof(TSource), "p");
            var prop = typeof(TSource).GetProperty(sortColumn);
            var propType = prop.PropertyType;
            //var sortExpression = Expression.Lambda<Func<TSource, object>>(Expression.Property(param, sortColumn), param);
            //var sortExpression =
            //    Expression.Lambda<Func<TSource, object>>(Expression.Convert(Expression.Property(param, sortColumn),
            //                                                                typeof (object)), param);
            if (propType == typeof(DateTime))
            {
                var sortExpression = Expression.Lambda<Func<TSource, DateTime>>(Expression.Property(param, sortColumn),
                    param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }
            if (propType == typeof(double))
            {
                var sortExpression = Expression.Lambda<Func<TSource, double>>(Expression.Property(param, sortColumn),
                    param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }

            if (propType == typeof(int))
            {
                var sortExpression = Expression.Lambda<Func<TSource, int>>(Expression.Property(param, sortColumn), param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }
            if (propType == typeof(string))
            {
                var sortExpression = Expression.Lambda<Func<TSource, string>>(Expression.Property(param, sortColumn),
                    param);
                return sortOrder == SortOrder.Ascending
                    ? source.OrderBy(sortExpression)
                    : source.OrderByDescending(sortExpression);
            }

            throw new NotSupportedException("Object type resolution not implemented for this type");
        }
    }
}