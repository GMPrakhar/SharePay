using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class GroupModel : BaseModel
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public required UserViewModel Owner { get; set; }

        public ISet<UserViewModel> Users { get; internal set; } = new HashSet<UserViewModel>();

        public ISet<TransactionModel> Transactions { get; internal set; } = new HashSet<TransactionModel>();
    }
}
