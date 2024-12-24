using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Business.Interfaces
{
    public interface IUserAccountBusiness
    {
        /// <summary>
        /// Logs the user in and returns an API key in response corresponding to the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Api Key for the user.</returns>
        Task<string> Login(string username, string password);
    }
}
