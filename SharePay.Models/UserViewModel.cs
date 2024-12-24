using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class UserViewModel : CredentialModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }

    }
}
