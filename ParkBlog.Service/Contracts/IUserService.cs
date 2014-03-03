using Microsoft.AspNet.Identity;
using ParkBlog.Domain;
using System.Threading.Tasks;

namespace ParkBlog.Service.Contracts
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(string userName, string password, string email);

        Task<User> GetUser(string userName, string password);
    }
}