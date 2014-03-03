using ParkBlog.Data.Contracts;
using ParkBlog.Domain;
using ParkBlog.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ParkBlog.Service.Implementation
{
    public class EventPostService : BaseService<EventPost>, IEventPostService
    {
        public EventPostService(IQueryableContext queryableContext)
            : base(queryableContext)
        {
        }

        public void AddEvent(EventPost eventPost)
        {
            eventPost.IsPublished = false;
            eventPost.AddedDate = DateTime.Now;

            AddItem(eventPost);
            Commit();
        }

        public void EditEvent(EventPost editedEvent)
        {
            var eventPost = FindItem(editedEvent.Id);

            editedEvent.AddedDate = eventPost.AddedDate;
            editedEvent.UserId = eventPost.UserId;

            EditItem(eventPost, editedEvent);
            Commit();
        }

        public void DeleteEvent(Expression<Func<EventPost, bool>> predicate)
        {
            DeleteItem(predicate);
            Commit();
        }

        public EventPost GetEvent(Expression<Func<EventPost, bool>> predicate)
        {
            var eventPost = GetItem(predicate);
            return eventPost;
        }

        public IEnumerable<EventPost> GetEvents(Expression<Func<EventPost, bool>> predicate = null)
        {
            var events = GetItems(predicate);
            return events;
        }
    }
}