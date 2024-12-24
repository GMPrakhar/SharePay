using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharePay.Business.Interfaces;
using SharePay.Models;
using System.Text.Json;

namespace SharePay.APIFunctions
{
    public class GroupController
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IUserGroupBusiness userGroupBusiness;
        private readonly IGroupBusiness groupBusiness;

        public GroupController(ILogger<GroupController> logger, IUserGroupBusiness userGroupBusiness, IGroupBusiness groupBusiness)
        {
            _logger = logger;
            this.userGroupBusiness = userGroupBusiness;
            this.groupBusiness = groupBusiness;
        }

        [Function(nameof(CreateGroupAsync))]
        public async Task<IActionResult> CreateGroupAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups")] HttpRequest req)
        {
            _logger.LogInformation("HTTP Trigger function to process group create");

            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var groupModel = await req.ReadFromJsonAsync<GroupModel>(jsonSettings);

            if(groupModel == null)
            {
                return new BadRequestObjectResult("Please provide a valid group model");
            }

            await this.userGroupBusiness.CreateGroup(groupModel);

            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function(nameof(ListGroupsAsync))]
        public async Task<IActionResult> ListGroupsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/user/{userId}/groups")] HttpRequest req,
            Guid userId)
        {
            _logger.LogInformation("HTTP Trigger function to get list of groups for the user");
            
            var groups = await this.userGroupBusiness.GetGroupsForUser(userId);

            return new OkObjectResult(groups);
        }

        [Function(nameof(GetGroupAsync))]
        public async Task<IActionResult> GetGroupAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups/{groupId}")] HttpRequest req)
        {
            _logger.LogInformation("HTTP Trigger function to process group create");
            
            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var userModel = await req.ReadFromJsonAsync<UserModel>(jsonSettings);

            if (userModel == null)
            {
                return new BadRequestObjectResult("Please provide a valid user model");
            }

           // await this.userGroupBusiness.Get(groupModel);

            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function(nameof(AddTransactionAsync))]
        public async Task<IActionResult> AddTransactionAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups/{groupId}/transactions")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to add a transaction to a group");

            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var transactionModel = await req.ReadFromJsonAsync<TransactionModel>(jsonSettings);

            if (transactionModel == null)
            {
                return new BadRequestObjectResult("Please provide a valid transaction model");
            }

            var result = await this.groupBusiness.AddTransaction(groupId, transactionModel);

            if (!result)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("Transaction added successfully");
        }

        [Function(nameof(GetConsolidatedTransactionAsync))]
        public async Task<IActionResult> GetConsolidatedTransactionAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}/transactions/consolidated")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to get consolidated transactions for a group");

            var consolidatedTransactions = await this.groupBusiness.GetConsolidatedTransactions(groupId);

            return new OkObjectResult(consolidatedTransactions);
        }

        [Function(nameof(GetTransactionsAsync))]
        public async Task<IActionResult> GetTransactionsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}/transactions")] HttpRequest req, Guid groupId)
        {
            // Get page number and size from request params
            var page = req.Query["page"];
            var size = req.Query["size"];

            if (string.IsNullOrEmpty(page) || string.IsNullOrEmpty(size))
            {
                return new BadRequestObjectResult("Page and size query parameters are required.");
            }
            
            var allTransactions = await this.groupBusiness.GetTransactions(groupId, int.Parse(page), int.Parse(size));
            _logger.LogInformation("HTTP Trigger function to get consolidated transactions for a group");

            return new OkObjectResult(allTransactions);
        }

        [Function(nameof(AddUserToGroupAsync))]
        public async Task<IActionResult> AddUserToGroupAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups/{groupId}/users/{userId}")] HttpRequest req, Guid groupId, Guid userId)
        {
            _logger.LogInformation("HTTP Trigger function to add a user to a group");

            var result = await this.userGroupBusiness.AddUserToGroup(userId, groupId);

            if (!result)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("User added to group successfully");
        }

        [Function(nameof(GetGroupUsersAsync))]
        public async Task<IActionResult> GetGroupUsersAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}/users")] HttpRequest req, Guid groupId)
        {
            var users = await groupBusiness.GetGroupUsersAsync(groupId);
            if (users == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(users);
        }
    }
}
