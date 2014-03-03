using ParkBlog.Data.Contracts;
using ParkBlog.Domain;
using ParkBlog.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ParkBlog.Service.Implementation
{
    public class NewsPostService : BaseService<NewsPost>, INewsPostService
    {
        public NewsPostService(IQueryableContext queryableContext)
            : base(queryableContext)
        {
        }

        public void AddNews(NewsPost news)
        {
            news.IsPublished = false;
            news.AddedDate = DateTime.Now;

            AddItem(news);
            Commit();
        }

        public void EditNews(NewsPost editedNews)
        {
            var newsPost = FindItem(editedNews.Id);

            editedNews.AddedDate = newsPost.AddedDate;
            editedNews.TotalLikes = newsPost.TotalLikes;
            editedNews.UserId = newsPost.UserId;

            EditItem(newsPost, editedNews);
            Commit();
        }

        public void DeleteNews(Expression<Func<NewsPost, bool>> predicate)
        {
            DeleteItem(predicate);
            Commit();
        }

        public NewsPost GetNews(Expression<Func<NewsPost, bool>> predicate)
        {
            var news = GetItem(predicate);
            return news;
        }

        public IEnumerable<NewsPost> GetNewses(Expression<Func<NewsPost, bool>> predicate = null)
        {
            var newses = GetItems(predicate);
            return newses;
        }
    }
}