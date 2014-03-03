using System;

namespace ParkBlog.Domain
{
    public class ArticlePost : BasePost
    {
        public DateTime? ModefiedDate { get; set; }

        public int RateValue { get; set; }

        public int TotalRates { get; set; }
    }
}