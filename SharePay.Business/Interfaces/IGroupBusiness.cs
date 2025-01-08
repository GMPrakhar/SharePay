using SharePay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Business.Interfaces
{
    public interface IGroupBusiness
    {
        Task<GroupModel> GetGroup(Guid userId, Guid groupId);

        Task<bool> UpdateGroup(Guid userId, GroupModel group);

        Task DeleteGroup(Guid userId, Guid groupId);

        Task<Guid> AddTransaction(Guid userId, Guid groupId, TransactionModel transaction);

        Task<ISet<BalancedTransactionModel>> GetConsolidatedTransactions(Guid userId, Guid groupId);

        Task<ISet<TransactionViewModel>> GetTransactions(Guid userId, Guid groupId, int page, int size);

        Task<IEnumerable<UserViewModel>> GetGroupUsersAsync(Guid userId, Guid groupId);
    }
}
