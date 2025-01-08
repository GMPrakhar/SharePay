using System.Threading.Tasks;
using SharePay.Business.Interfaces;
using SharePay.Repository;
using SharePay.Models;

namespace SharePay.Business.Implementations
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUser(UserModel userModel)
        {
            await userRepository.AddUser(userModel);
        }

        public async Task<UserViewModel?> GetUserByEmail(string email)
        {
            return await userRepository.GetUserByEmail(email);
        }

        public async Task<List<UserViewModel>> GetUsersByEmail(List<string> emails)
        {
            return await userRepository.GetUsersByEmail(emails);
        }
    }
}