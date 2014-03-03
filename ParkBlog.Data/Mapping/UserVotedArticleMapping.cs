using ParkBlog.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ParkBlog.Data.Mapping
{
    internal class UserVotedArticleMapping : EntityTypeConfiguration<UserVotedArticle>
    {
        public UserVotedArticleMapping()
        {
            HasKey(k => new { k.ArticleId, k.UserId });
        }
    }
}