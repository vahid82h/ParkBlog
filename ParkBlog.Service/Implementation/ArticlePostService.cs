using System.Collections.Generic;
using ParkBlog.Data.Contracts;
using ParkBlog.Domain;
using ParkBlog.Service.Contracts;
using System;
using System.Linq.Expressions;

namespace ParkBlog.Service.Implementation
{
    public class ArticlePostService : BaseService<ArticlePost>, IArticlePostService
    {
        public ArticlePostService(IQueryableContext queryableContext)
            : base(queryableContext)
        {
        }

        public void AddArticle(ArticlePost article)
        {
            article.IsPublished = false;
            article.AddedDate = DateTime.Now;

            AddItem(article);
            Commit();
        }

        public void EditArticle(ArticlePost editedArticle)
        {
            var articleInDb = FindItem(editedArticle.Id);

            editedArticle.ModifiedDate = DateTime.Now;
            editedArticle.AddedDate = articleInDb.AddedDate;
            editedArticle.IsPublished = articleInDb.IsPublished;
            editedArticle.RateValue = articleInDb.RateValue;
            editedArticle.TotalRates = articleInDb.TotalRates;
            editedArticle.UserId = articleInDb.UserId;

            EditItem(articleInDb, editedArticle);
            Commit();
        }

        public void DeleteArticle(Expression<Func<ArticlePost, bool>> predicate)
        {
            DeleteItem(predicate);
            Commit();
        }

        public ArticlePost GetArticle(Expression<Func<ArticlePost, bool>> predicate)
        {
            var article = GetItem(predicate);
            return article;
        }

        public IEnumerable<ArticlePost> GetArticles(Expression<Func<ArticlePost, bool>> predicate = null)
        {
            var articles = GetItems(predicate);
            return articles;
        }
    }
}