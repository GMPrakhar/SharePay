using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class UserModel : UserViewModel
    {
        public ISet<GroupModel> Groups { get; set; } = new HashSet<GroupModel>();
    }
}
