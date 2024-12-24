using SharePay.Business.Interfaces;
using SharePay.Models;
using SharePay.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharePay.Business.Implementations
{
    public class GroupBusiness : IGroupBusiness
    {
        private readonly IGroupRepository _groupRepository;

        public GroupBusiness(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<GroupModel> GetGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateGroup(GroupModel group)
        {
            return await _groupRepository.UpdateGroup(group);
        }

        public async Task DeleteGroup(Guid groupId)
        {
            await _groupRepository.DeleteGroup(groupId);
        }

        public Task<bool> AddTransaction(Guid groupId, TransactionModel transaction)
        {
            return _groupRepository.AddTransaction(groupId, transaction);
        }

        public Task<ISet<BalancedTransactionModel>> GetConsolidatedTransactions(Guid groupId)
        {
            return _groupRepository.GetConsolidatedTransactions(groupId);
        }

        public Task<ISet<TransactionViewModel>> GetTransactions(Guid groupId, int page, int size)
        {
            return _groupRepository.GetTransactions(groupId, page, size);
        }

        public Task<IEnumerable<UserViewModel>> GetGroupUsersAsync(Guid groupId)
        {
            return _groupRepository.GetGroupUsersAsync(groupId);
        }
    }
}
