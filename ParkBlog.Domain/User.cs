using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ParkBlog.Domain
{
    public class User : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public virtual Profile Profile { get; set; }

        public virtual ICollection<BasePost> Posts { get; set; }
    }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole()
        { }

        public CustomRole(string name)
        {
            Name = name;
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }

    public class CustomUserClaim : IdentityUserClaim<int> { }

    public class CustomUserLogin : IdentityUserLogin<int> { }
}