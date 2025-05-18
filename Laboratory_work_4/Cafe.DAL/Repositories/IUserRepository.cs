using System.Threading.Tasks;
using Cafe.DAL.Entities;

namespace Cafe.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
    }
}