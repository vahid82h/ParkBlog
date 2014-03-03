using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ParkBlog.Data;
using ParkBlog.Domain;
using ParkBlog.Service.Contracts;
using System.Threading.Tasks;

namespace ParkBlog.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User, int> _userManager;

        public UserService()
        {
            _userManager =
                new UserManager<User, int>(
                    new UserStore<User, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(
                        new DataContext()));
        }

        public async Task<User> GetUser(string userName, string password)
        {
            var user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<IdentityResult> CreatUser(string userName, string pasword, string email)
        {
            var user = new User
            {
                Email = email,
                UserName = userName,
                //PhoneNumber = "09190086043",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false
            };

            var result = await _userManager.CreateAsync(user, pasword);
            return result;
        }
    }
}