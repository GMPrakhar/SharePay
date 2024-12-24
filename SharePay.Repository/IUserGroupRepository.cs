using SharePay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Repository
{
    public interface IUserGroupRepository
    {
        Task<ISet<GroupModel>> GetGroupsForUser(Guid userId);

        Task<bool> CreateGroup(GroupModel group);

        Task<bool> AddUserToGroup(Guid userId, Guid groupId);

        Task<bool> RemoveUserFromGroup(Guid userId, Guid groupId);
    }
}
