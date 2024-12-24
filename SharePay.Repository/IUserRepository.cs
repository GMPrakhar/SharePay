using System.Threading.Tasks;
using SharePay.Models;

namespace SharePay.Repository
{
    public interface IUserRepository
    {
        Task AddUser(UserModel userModel);
        Task<UserModel?> GetUserByEmail(string email);
    }
}