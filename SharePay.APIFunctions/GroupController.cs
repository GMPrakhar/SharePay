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
        private readonly IUserGroupBusiness _userGroupBusiness;
        private readonly IGroupBusiness _groupBusiness;

        public GroupController(ILogger<GroupController> logger, IUserGroupBusiness userGroupBusiness, IGroupBusiness groupBusiness)
        {
            _logger = logger;
            _userGroupBusiness = userGroupBusiness;
            _groupBusiness = groupBusiness;
        }

        [Function(nameof(CreateGroupAsync))]
        public async Task<IActionResult> CreateGroupAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups")] HttpRequest req)
        {
            _logger.LogInformation("HTTP Trigger function to process group create");

            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var groupModel = await JsonSerializer.DeserializeAsync<GroupModel>(req.Body, jsonSettings);

            if (groupModel == null)
            {
                return new BadRequestObjectResult("Please provide a valid group model");
            }

            var result = await _userGroupBusiness.CreateGroup(groupModel);

            return new OkObjectResult(new object());
        }

        [Function(nameof(ListGroupsAsync))]
        public async Task<IActionResult> ListGroupsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/user/{userId}/groups")] HttpRequest req, Guid userId)
        {
            _logger.LogInformation("HTTP Trigger function to get list of groups for the user");

            var groups = await _userGroupBusiness.GetGroupsForUser(userId);

            return new OkObjectResult(groups);
        }

        [Function(nameof(GetGroupAsync))]
        public async Task<IActionResult> GetGroupAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to get group details");

            var userId = req.HttpContext.User.Identity.Name ?? req.Query["userId"]; ;
            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestObjectResult("UserId query parameter is required.");
            }

            var group = await _groupBusiness.GetGroup(Guid.Parse(userId), groupId);

            if (group == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(group);
        }

        [Function(nameof(AddTransactionAsync))]
        public async Task<IActionResult> AddTransactionAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups/{groupId}/transactions")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to add a transaction to a group");

            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var transactionModel = await JsonSerializer.DeserializeAsync<TransactionModel>(req.Body, jsonSettings);

            if (transactionModel == null)
            {
                return new BadRequestObjectResult("Please provide a valid transaction model");
            }

            var userId = req.HttpContext.User.Identity.Name ?? req.Query["userId"];
            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestObjectResult("UserId query parameter is required.");
            }

            var result = await _groupBusiness.AddTransaction(Guid.Parse(userId), groupId, transactionModel);
            transactionModel.Id = result;

            if (result == Guid.Empty)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(transactionModel);
        }

        [Function(nameof(GetConsolidatedTransactionAsync))]
        public async Task<IActionResult> GetConsolidatedTransactionAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}/transactions/consolidated")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to get consolidated transactions for a group");

            var userId = req.HttpContext.User.Identity.Name ?? req.Query["userId"] ;
            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestObjectResult("UserId query parameter is required.");
            }

            var consolidatedTransactions = await _groupBusiness.GetConsolidatedTransactions(Guid.Parse(userId), groupId);

            return new OkObjectResult(consolidatedTransactions);
        }

        [Function(nameof(GetTransactionsAsync))]
        public async Task<IActionResult> GetTransactionsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}/transactions")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to get transactions for a group");

            var page = req.Query["page"];
            var size = req.Query["size"];

            if (string.IsNullOrEmpty(page) || string.IsNullOrEmpty(size))
            {
                return new BadRequestObjectResult("Page and size query parameters are required.");
            }

            var userId = req.HttpContext.User.Identity.Name ?? req.Query["userId"];
            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestObjectResult("UserId query parameter is required.");
            }

            var allTransactions = await _groupBusiness.GetTransactions(Guid.Parse(userId), groupId, int.Parse(page), int.Parse(size));

            return new OkObjectResult(allTransactions);
        }

        [Function(nameof(AddUserToGroupAsync))]
        public async Task<IActionResult> AddUserToGroupAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups/{groupId}/users/{userId}")] HttpRequest req, Guid groupId, Guid userId)
        {
            _logger.LogInformation("HTTP Trigger function to add a user to a group");

            var result = await _userGroupBusiness.AddUserToGroup(userId, groupId);

            if (!result)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("User added to group successfully");
        }

        [Function(nameof(AddMultipleUsersToGroupAsync))]
        public async Task<IActionResult> AddMultipleUsersToGroupAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/groups/{groupId}/users")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to add multiple users to a group");

            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            var userIds = await JsonSerializer.DeserializeAsync<List<Guid>>(req.Body, jsonSettings);

            if (userIds == null || !userIds.Any())
            {
                return new BadRequestObjectResult("Please provide a valid list of user IDs");
            }

            var result = await _userGroupBusiness.AddMultipleUsersToGroup(userIds, groupId);

            if (!result)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("Users added to group successfully");
        }

        [Function(nameof(GetGroupUsersAsync))]
        public async Task<IActionResult> GetGroupUsersAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/groups/{groupId}/users")] HttpRequest req, Guid groupId)
        {
            _logger.LogInformation("HTTP Trigger function to get users of a group");

            var userId = req.HttpContext.User.Identity.Name ?? req.Query["userId"]; ;
            if (string.IsNullOrEmpty(userId))
            {
                return new BadRequestObjectResult("UserId query parameter is required.");
            }

            var users = await _groupBusiness.GetGroupUsersAsync(Guid.Parse(userId), groupId);

            if (users == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(users);
        }
    }
}
