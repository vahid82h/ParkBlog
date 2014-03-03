using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    internal class EventPostMapping : EntityTypeConfiguration<EventPost>
    {
        public EventPostMapping()
        {
            ToTable("EventPost");
            HasKey(t => t.Id);
            Property(t => t.AddedDate).IsRequired();
            Property(t => t.Description).IsUnicode(true).IsRequired();
            Property(t => t.EventDate).IsRequired();
            Property(t => t.IsPublished).IsRequired();
            Property(t => t.Place).HasMaxLength(100).IsUnicode(true).IsRequired();
            Property(t => t.Title).HasMaxLength(500).IsUnicode(true).IsRequired();
        }
    }
}