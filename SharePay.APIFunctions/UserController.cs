using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharePay.Business.Interfaces;
using SharePay.Models;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharePay.APIFunctions
{
    public class UserController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserBusiness userBusiness;

        public UserController(ILogger<UserController> logger, IUserBusiness userBusiness)
        {
            _logger = logger;
            this.userBusiness = userBusiness;
        }

        [Function(nameof(AddUserAsync))]
        public async Task<IActionResult> AddUserAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/users")] HttpRequest req)
        {
            _logger.LogInformation("HTTP Trigger function to process user add");
            var userModel = await req.ReadFromJsonAsync<UserModel>();

            if (userModel == null)
            {
                return new BadRequestObjectResult("Please provide a valid user model");
            }

            await this.userBusiness.AddUser(userModel);
            return new OkObjectResult("User added successfully");
        }

        [Function(nameof(GetUserAsync))]
        public async Task<IActionResult> GetUserAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/users/{email}")] HttpRequest req, string email)
        {
            _logger.LogInformation("HTTP Trigger function to get user by email");

            var user = await this.userBusiness.GetUserByEmail(email);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found");
            }

            return new OkObjectResult(user);
        }

        [Function(nameof(GetUsersAsync))]
        public async Task<IActionResult> GetUsersAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/users/list")] HttpRequest req)
        {
            _logger.LogInformation("HTTP Trigger function to get user by email");
            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var userEmails = await JsonSerializer.DeserializeAsync<List<string>>(req.Body, jsonSettings);

            var user = await this.userBusiness.GetUsersByEmail(userEmails);

            if (user == null)
            {
                return new NotFoundObjectResult("Users not found");
            }

            return new OkObjectResult(user);
        }
    }
}
