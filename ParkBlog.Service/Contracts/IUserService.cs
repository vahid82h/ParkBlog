using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ParkBlog.Domain;

namespace ParkBlog.Service.Contracts
{
    public interface IUserService
    {
        Task<User> GetUser(string userName, string password);

        Task<IdentityResult> CreatUser(string userName, string password, string email);
    }
}