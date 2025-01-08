using SharePay.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Repository
{
    internal class UserGroupRepository : IUserGroupRepository
    {
        public async Task<bool> AddUserToGroup(Guid userId, Guid groupId)
        {
            using var connection = new SqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            // Insert user into Group_Users
            command.CommandText = "INSERT INTO Group_Users (user_id, group_id, added_by_user_id, balance) VALUES (@userId, @groupId, @addedByUserId, @balance)";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@groupId", groupId);
            command.Parameters.AddWithValue("@addedByUserId", userId); // Assuming the user is adding themselves, update this with AppContext.CurrentUser in future.
            command.Parameters.AddWithValue("@balance", 0);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> CreateGroup(GroupModel group)
        {
            using var connection = new SqlConnection(Database.ConnectionString);
            connection.Open();
            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var command = connection.CreateCommand();
                command.Transaction = transaction;

                var groupId = Guid.NewGuid();

                // Insert into Groups and retrieve the inserted ID
                command.CommandText = "INSERT INTO Groups (name, owner_id, group_id) VALUES (@value1, @value2, @value3);";
                command.Parameters.AddWithValue("@value1", group.Name);
                command.Parameters.AddWithValue("@value2", group.Owner.Id);
                command.Parameters.AddWithValue("@value3", groupId);

                await command.ExecuteNonQueryAsync();  // Retrieve the new group ID

                // Prepare and execute the second command for Group_Users
                command.CommandText = "INSERT INTO Group_Users (user_id, group_id, added_by_user_id, balance) VALUES (@value3, @value4, @value5, @value6)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@value3", group.Owner.Id);
                command.Parameters.AddWithValue("@value4", groupId);
                command.Parameters.AddWithValue("@value5", group.Owner.Id);
                command.Parameters.AddWithValue("@value6", 0);

                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<ISet<GroupModel>> GetGroupsForUser(Guid userId)
        {
            using var connection = new SqlConnection(
                Database.ConnectionString);
            await connection.OpenAsync();
            using var command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Groups grps " +
                "INNER JOIN Group_Users guser " +
                "ON grps.group_id = guser.group_id AND guser.user_id = @value1";
            command.Parameters.AddWithValue("@value1", userId);
            using var reader = await command.ExecuteReaderAsync();
            var groups = new HashSet<GroupModel>();
            while (await reader.ReadAsync()) {
                var group = new GroupModel() { Name = reader.GetString(1), Owner = new UserViewModel() { Id = userId, Name = "", Email = "" } };
                group.Id = reader.GetGuid(0);
                groups.Add(group);
            }

            return groups;
        }

        public async Task<bool> RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            using var connection = new SqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            // Check if the balance is 0 for the user
            command.Connection = connection;
            command.CommandText = "SELECT balance FROM Group_Users WHERE user_id = @userId AND group_id = @groupId";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@groupId", groupId);
            var balance = (decimal)await command.ExecuteScalarAsync();
            if(balance != 0)
            {
                throw new InvalidOperationException("User has a balance in the group. Cannot remove user.");
            }

            // Delete user from Group_Users
            command.CommandText = "DELETE FROM Group_Users WHERE user_id = @userId AND group_id = @groupId";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@groupId", groupId);

            return await command.ExecuteNonQueryAsync() > 0;
        }
        
        public async Task<bool> AddMultipleUsersToGroup(List<Guid> userIds, Guid groupId)
        {
            using var connection = new SqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = connection.CreateCommand();
                command.Transaction = transaction;

                var values = new StringBuilder();
                for (int i = 0; i < userIds.Count; i++)
                {
                    values.Append($"(@userId{i}, @groupId, @addedByUserId{i}, @balance),");
                    command.Parameters.AddWithValue($"@userId{i}", userIds[i]);
                    command.Parameters.AddWithValue($"@addedByUserId{i}", userIds[i]); // Assuming the user is adding themselves, update this with AppContext.CurrentUser in future.
                }
                values.Length--; // Remove the last comma

                command.CommandText = $"INSERT INTO Group_Users (user_id, group_id, added_by_user_id, balance) VALUES {values}";
                command.Parameters.AddWithValue("@groupId", groupId);
                command.Parameters.AddWithValue("@balance", 0);

                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
