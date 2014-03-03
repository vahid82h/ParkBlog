using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    public class TagMapping : EntityTypeConfiguration<Tag>
    {
        public TagMapping()
        {
            HasKey(t => t.Id);
            Property(t => t.Name).HasMaxLength(20).IsUnicode(false).IsRequired();
            Property(t => t.UrlSlug).HasMaxLength(100).IsUnicode(false).IsRequired();
            Property(t => t.Description).HasMaxLength(500).IsUnicode(true).IsRequired();

            HasMany(t => t.Posts).WithMany(t => t.Tags);
        }
    }
}