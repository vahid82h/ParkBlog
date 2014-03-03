using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    internal class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            Property(t => t.UserName).HasMaxLength(50).IsUnicode(false).IsRequired();
            Property(t => t.Email).HasMaxLength(50).IsUnicode(false).IsRequired();
            Property(t => t.PhoneNumber).HasMaxLength(50).IsUnicode(false).IsOptional();

            HasMany(t => t.Posts).WithRequired(t => t.Owner).HasForeignKey(t => t.UserId).WillCascadeOnDelete(false);
        }
    }
}