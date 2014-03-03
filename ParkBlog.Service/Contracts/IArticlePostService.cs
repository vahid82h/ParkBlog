using ParkBlog.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ParkBlog.Service.Contracts
{
    /// <summary>
    /// Interface IArticlePostService
    /// </summary>
    public interface IArticlePostService
    {
        /// <summary>
        /// Adds the article.
        /// </summary>
        /// <param name="article">The article.</param>
        void AddArticle(ArticlePost article);

        /// <summary>
        /// Edits the article.
        /// </summary>
        /// <param name="editedArticle">The edited article.</param>
        void EditArticle(ArticlePost editedArticle);

        /// <summary>
        /// Deletes the article.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        void DeleteArticle(Expression<Func<ArticlePost, bool>> predicate);

        /// <summary>
        /// Gets the article.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>ArticlePost.</returns>
        ArticlePost GetArticle(Expression<Func<ArticlePost, bool>> predicate);

        /// <summary>
        /// Gets the articles.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IEnumerable{ArticlePost}.</returns>
        IEnumerable<ArticlePost> GetArticles(Expression<Func<ArticlePost, bool>> predicate = null);
    }
}