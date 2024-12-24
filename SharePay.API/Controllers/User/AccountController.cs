using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharePay.Models;
using SharePay.Business.Interfaces;

namespace SharePay.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserAccountBusiness userAccountBusiness;

        public AccountController(ILogger<AccountController> logger, IUserAccountBusiness userAccountBusiness)
        {
            _logger = logger;
            this.userAccountBusiness = userAccountBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] CredentialModel creds)
        {
            try
            {
                var apiKey = await this.userAccountBusiness.Login(creds.Username, creds.Password);
                return this.Ok(apiKey);
            }
            catch (AccessViolationException)
            {
                return this.Unauthorized();
            }
        }
    }
}
