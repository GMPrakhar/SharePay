using System.Threading.Tasks;
using SharePay.Models;

namespace SharePay.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task AddUser(UserModel userModel);
        Task<UserViewModel?> GetUserByEmail(string email);
    }
}