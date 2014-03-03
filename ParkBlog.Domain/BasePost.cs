using System;
using System.Collections.Generic;

namespace ParkBlog.Domain
{
    public abstract class BasePost : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }

        public bool IsPublished { get; set; }

        public int UserId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}