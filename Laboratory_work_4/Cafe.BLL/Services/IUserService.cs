using Cafe.DAL.Entities;
using System.Threading.Tasks;

namespace Cafe.BLL.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string username, string password);
        Task<User> LoginAsync(string username, string password);
    }
}