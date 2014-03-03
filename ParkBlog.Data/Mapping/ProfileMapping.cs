using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    internal class ProfileMapping : EntityTypeConfiguration<Profile>
    {
        public ProfileMapping()
        {
            HasKey(t => t.Id);
            Property(t => t.FirstName).HasMaxLength(20).IsUnicode(true).IsRequired();
            Property(t => t.LastName).HasMaxLength(20).IsUnicode(true).IsRequired();
            Property(t => t.Website).HasMaxLength(50).IsUnicode(false).IsOptional();

            HasRequired(t => t.User).WithOptional(x => x.Profile);
        }
    }
}