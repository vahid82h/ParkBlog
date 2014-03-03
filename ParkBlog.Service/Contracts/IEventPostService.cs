using ParkBlog.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ParkBlog.Service.Contracts
{
    /// <summary>
    /// Interface IEventPostService
    /// </summary>
    public interface IEventPostService
    {
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="article">The article.</param>
        void AddEvent(EventPost article);

        /// <summary>
        /// Edits the event.
        /// </summary>
        /// <param name="editedEvent">The edited event.</param>
        void EditEvent(EventPost editedEvent);

        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        void DeleteEvent(Expression<Func<EventPost, bool>> predicate);

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>EventPost.</returns>
        EventPost GetEvent(Expression<Func<EventPost, bool>> predicate);

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IEnumerable{EventPost}.</returns>
        IEnumerable<EventPost> GetEvents(Expression<Func<EventPost, bool>> predicate = null);
    }
}