using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class CredentialModel : BaseModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
