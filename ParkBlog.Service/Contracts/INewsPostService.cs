using ParkBlog.Domain;
using System;
using System.Linq.Expressions;

namespace ParkBlog.Service.Contracts
{
    /// <summary>
    /// Interface INewsPostService
    /// </summary>
    public interface INewsPostService
    {
        /// <summary>
        /// Adds the news.
        /// </summary>
        /// <param name="article">The article.</param>
        void AddNews(NewsPost article);

        /// <summary>
        /// Edits the news.
        /// </summary>
        /// <param name="editedNews">The edited news.</param>
        void EditNews(NewsPost editedNews);

        /// <summary>
        /// Deletes the news.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        void DeleteNews(Expression<Func<NewsPost, bool>> predicate);

        /// <summary>
        /// Gets the news.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>NewsPost.</returns>
        NewsPost GetNews(Expression<Func<NewsPost, bool>> predicate);

        /// <summary>
        /// Gets the newses.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>NewsPost.</returns>
        NewsPost GetNewses(Expression<Func<NewsPost, bool>> predicate = null);
    }
}