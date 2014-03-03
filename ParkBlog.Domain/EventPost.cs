using System;

namespace ParkBlog.Domain
{
    public class EventPost : BasePost
    {
        public DateTime EventDate { get; set; }

        public string Place { get; set; }
    }
}