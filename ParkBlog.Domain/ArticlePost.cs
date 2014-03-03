using System;

namespace ParkBlog.Domain
{
    public class ArticlePost : BasePost
    {
        public DateTime? ModifiedDate { get; set; }

        public int RateValue { get; set; }

        public int TotalRates { get; set; }
    }
}