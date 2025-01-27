﻿using SharePay.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Repository
{
    public class GroupRepository : IGroupRepository
    {
        public async Task<Guid> AddTransaction(Guid userId, Guid groupId, TransactionModel transactionModel)
        {
            using var connection = new SqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                using var command = connection.CreateCommand();
                command.Transaction = transaction;

                // Insert into Transactions table
                var transactionId = Guid.NewGuid();
                command.CommandText = @"
                    INSERT INTO Transactions (transaction_id, group_id, recording_user, transaction_type, name, total_amount)
                    VALUES (@transactionId, @groupId, @fromUserId, @category, @name, @totalAmount)";
                command.Parameters.AddWithValue("@transactionId", transactionId);
                command.Parameters.AddWithValue("@groupId", groupId);
                command.Parameters.AddWithValue("@fromUserId", transactionModel.FromUser);
                command.Parameters.AddWithValue("@category", transactionModel.Category);
                command.Parameters.AddWithValue("@name", transactionModel.Description);
                command.Parameters.AddWithValue("@totalAmount", transactionModel.TotalAmount);

                await command.ExecuteNonQueryAsync();

                // Insert into Transaction_Details table
                var values = new StringBuilder();

                foreach (var toUser in transactionModel.ToUsers)
                {
                    if (values.Length > 0)
                        values.Append(", ");

                    values.Append($"('{transactionId}', '{toUser}', {transactionModel.PerUserAmount[toUser]}, '{transactionModel.FromUser}', '{transactionModel.FromUser}')");
                }

                command.CommandText = $@"
                    INSERT INTO Transaction_Details (transaction_id, user_id, owed_amount, created_by_user_id, updated_by_user_id)
                        VALUES {values}";

                await command.ExecuteNonQueryAsync();

                // Update balances in Group_Users table
                var updateCases = new StringBuilder();
                var userIds = new StringBuilder();

                // Update balance for the user who made the transaction
                updateCases.Append($"WHEN user_id = '{transactionModel.FromUser}' THEN balance - {transactionModel.TotalAmount} ");
                userIds.Append($"'{transactionModel.FromUser}'");

                // Update balance for each user who received the transaction
                foreach (var toUser in transactionModel.ToUsers)
                {
                    updateCases.Append($"WHEN user_id = '{toUser}' THEN balance + {transactionModel.PerUserAmount[toUser]} ");
                    userIds.Append($", '{toUser}'");
                }

                command.CommandText = $@"
                    UPDATE Group_Users
                    SET balance = CASE {updateCases.ToString()}
                    END
                    WHERE user_id IN ({userIds.ToString()}) AND group_id = @groupId";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@groupId", groupId);
                await command.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return transactionId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return Guid.Empty;
            }
        }

        public Task DeleteGroup(Guid userId, Guid groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<ISet<BalancedTransactionModel>> GetConsolidatedTransactions(Guid userId, Guid groupId)
        {
            // Get the list of all users balances in the group

            using var connection = new SqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();

            // Check if the balance is 0 for the user
            command.Connection = connection;
            command.CommandText = "SELECT u.user_id, balance, u.Name FROM Group_Users gu " +
                "INNER JOIN Users u on gu.user_id = u.user_id " +
                $"WHERE gu.group_id = '{groupId}'";

            HashSet<TransactionModel> transactions = new HashSet<TransactionModel>();
            IDictionary<Guid, decimal> userBalanceMap = new Dictionary<Guid, decimal>();
            IDictionary<Guid, String> userNameMap = new Dictionary<Guid, String>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader.GetGuid(0);
                var balance = reader.GetDecimal(1);
                var name = reader.GetString(2);

                userBalanceMap.Add(id, balance);
                userNameMap.Add(id, name);
            }

            // Create a list of transactions that needs to be done based on the balances in the userBalanceMap. The balance will be positive or negative depending on whether the user would get the money or send the money.
            var transactionList = TransactionBalancer.CalculateTransactions(userBalanceMap);
            foreach (var transaction in transactionList)
            {
                transaction.FromName = userNameMap[transaction.From];
                transaction.ToName = userNameMap[transaction.To];
            }

            return transactionList.ToHashSet();

        }

        public Task<GroupModel> GetGroup(Guid userId, Guid groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<ISet<TransactionViewModel>> GetTransactions(Guid userId, Guid groupId, int page, int size)
        {
            {
                using var connection = new SqlConnection(Database.ConnectionString);
                await connection.OpenAsync();
                using var command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT t.transaction_id, name, recording_user, transaction_type, t.total_amount
                    , CASE WHEN t.recording_user = @userId THEN
                            (Select CAST(SUM(owed_amount) AS VARCHAR)
                            FROM transaction_details td
                            WHERE td.user_id <> @userId
                            AND t.transaction_id = td.transaction_id)
                        ELSE 
                            (Select CAST(-1*SUM(owed_amount) AS VARCHAR)
                            FROM transaction_details td
                            WHERE td.user_id = @userId
                            AND t.transaction_id = td.transaction_id)
                        END As TranasctionInfo
                    , t.created_at
                    FROM Transactions t
                    WHERE group_id = @groupId
                    ORDER BY t.created_at DESC
                    OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY";

                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@groupId", groupId);
                command.Parameters.AddWithValue("@offset", (page - 1) * size);
                command.Parameters.AddWithValue("@size", size);

                var transactions = new HashSet<TransactionViewModel>();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var transaction = new TransactionViewModel
                    {
                        Id = reader.GetGuid(0),
                        Description = reader.GetString(1),
                        FromUser = reader.GetGuid(2),
                        Category = Enum.Parse<TransactionCategory>(reader.GetString(3)),
                        TotalAmount = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                        TransactionInfo = reader.IsDBNull(5) ? "0" : reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    };

                    transactions.Add(transaction);
                }

                return transactions;
            }

        }

        public async Task<IEnumerable<UserViewModel>> GetGroupUsersAsync(Guid userId, Guid groupId)
        {
            var users = new List<UserViewModel>();

            using var connection = new SqlConnection(Database.ConnectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT u.user_id, u.Name, u.Email FROM Group_Users gu " +
                                  "INNER JOIN Users u ON gu.user_id = u.user_id " +
                                  "WHERE gu.group_id = @GroupId";
            command.Parameters.AddWithValue("@GroupId", groupId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new UserViewModel
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }

            return users;
        }

        public Task<bool> UpdateGroup(Guid userId, GroupModel group)
        {
            throw new NotImplementedException();
        }
    }
}
