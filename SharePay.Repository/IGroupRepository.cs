using SharePay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Repository
{
    public interface IGroupRepository
    {
        Task<GroupModel> GetGroup(Guid groupId);

        Task<bool> UpdateGroup(GroupModel group);

        Task DeleteGroup(Guid groupId);

        Task<bool> AddTransaction(Guid groupId, TransactionModel transaction);

        Task<ISet<BalancedTransactionModel>> GetConsolidatedTransactions(Guid groupId);

        Task<ISet<TransactionViewModel>> GetTransactions(Guid groupId, int page, int size);

        Task<IEnumerable<UserViewModel>> GetGroupUsersAsync(Guid groupId);
    }
}
