using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    internal class NewsPostMapping : EntityTypeConfiguration<NewsPost>
    {
        public NewsPostMapping()
        {
            HasKey(t => t.Id);
            ToTable("NewsPost");
            Property(t => t.AddedDate).IsRequired();
            Property(t => t.Description).IsUnicode(true).IsRequired();
            Property(t => t.IsPublished).IsRequired();
            Property(t => t.TotalLikes).IsOptional();
            Property(t => t.Title).HasMaxLength(500).IsUnicode(true).IsRequired();
        }
    }
}