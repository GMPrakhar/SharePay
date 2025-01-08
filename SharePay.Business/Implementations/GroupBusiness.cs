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

        public async Task<GroupModel> GetGroup(Guid userId, Guid groupId)
        {
            return await _groupRepository.GetGroup(userId, groupId);
        }

        public async Task<bool> UpdateGroup(Guid userId, GroupModel group)
        {
            return await _groupRepository.UpdateGroup(userId, group);
        }

        public async Task DeleteGroup(Guid userId, Guid groupId)
        {
            await _groupRepository.DeleteGroup(userId, groupId);
        }

        public Task<Guid> AddTransaction(Guid userId, Guid groupId, TransactionModel transaction)
        {
            return _groupRepository.AddTransaction(userId, groupId, transaction);
        }

        public Task<ISet<BalancedTransactionModel>> GetConsolidatedTransactions(Guid userId, Guid groupId)
        {
            return _groupRepository.GetConsolidatedTransactions(userId, groupId);
        }

        public Task<ISet<TransactionViewModel>> GetTransactions(Guid userId, Guid groupId, int page, int size)
        {
            return _groupRepository.GetTransactions(userId, groupId, page, size);
        }

        public Task<IEnumerable<UserViewModel>> GetGroupUsersAsync(Guid userId, Guid groupId)
        {
            return _groupRepository.GetGroupUsersAsync(userId, groupId);
        }
    }
}
