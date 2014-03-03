using System.Collections.Generic;

namespace ParkBlog.Domain
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public string UrlSlug { get; set; }

        public string Description { get; set; }

        public virtual ICollection<BasePost> Posts { get; set; }
    }
}