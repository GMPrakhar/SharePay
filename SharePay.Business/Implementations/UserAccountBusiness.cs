using SharePay.Business.Interfaces;
using SharePay.Models;

namespace SharePay.Business.Implementations
{
    internal class UserAccountBusiness : IUserAccountBusiness
    {
        IDictionary<(string, string), string> _credToApiKey;

        public static IDictionary<string, UserModel> accounts = new Dictionary<string, UserModel>();

        public UserAccountBusiness()
        {
            _credToApiKey = new Dictionary<(string, string), string>()
            {
                { ("user", "pass"), "ApiKey" },
            };

        }

        static UserAccountBusiness()
        {

            var user = new UserModel()
            {
                Name = "Steph Toub",
                Email = "steph@ms.com"
            };

            var groupModel = new GroupModel() { Name = "Temp Group", Id = Guid.NewGuid(), Owner =  user };

            user.Groups.Add(groupModel);
            groupModel.Users.Add(user);

            accounts = new Dictionary<string, UserModel>()
            {
                { "ApiKey", user }
            };

        }

        public Task<string> Login(string username, string password)
        {
            if (_credToApiKey.ContainsKey((username, password)))
            {
                return Task.FromResult(_credToApiKey[(username, password)]);
            }

            throw new AccessViolationException();
        }
    }
}
