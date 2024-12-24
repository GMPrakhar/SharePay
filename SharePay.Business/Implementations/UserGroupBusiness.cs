using SharePay.Business.Interfaces;
using SharePay.Models;
using SharePay.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Business.Implementations
{
    internal class UserGroupBusiness : IUserGroupBusiness
    {
        private readonly IUserGroupRepository userGroupRepository;

        public UserGroupBusiness(IUserGroupRepository userGroupRepository)
        {
            this.userGroupRepository = userGroupRepository;
        }

        public Task<bool> AddUserToGroup(Guid userId, Guid groupId)
        {
            return this.userGroupRepository.AddUserToGroup(userId, groupId);
        }

        public Task<bool> CreateGroup(GroupModel group)
        {
            return this.userGroupRepository.CreateGroup(group);
        }

        public Task<ISet<GroupModel>> GetGroupsForUser(Guid userId)
        {
            return this.userGroupRepository.GetGroupsForUser(userId);
        }

        public Task<bool> RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            throw new NotImplementedException();
        }
    }
}
