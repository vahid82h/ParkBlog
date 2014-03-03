using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    internal class ArticlePostMapping : EntityTypeConfiguration<ArticlePost>
    {
        public ArticlePostMapping()
        {
            ToTable("ArticlePost");
            HasKey(t => t.Id);
            Property(t => t.AddedDate).IsRequired();
            Property(t => t.Description).IsUnicode(true).IsRequired();
            Property(t => t.IsPublished).IsRequired();
            Property(t => t.ModefiedDate).IsOptional();
            Property(t => t.RateValue).IsRequired();
            Property(t => t.Title).HasMaxLength(500).IsUnicode(true).IsRequired();
            Property(t => t.TotalRates).IsRequired();
        }
    }
}