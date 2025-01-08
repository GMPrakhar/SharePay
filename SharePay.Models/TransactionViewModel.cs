using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class TransactionViewModel : BaseModel
    {
        public Guid FromUser { get; set; }

        public decimal TotalAmount { get; set; }

        public required string Description { get; set; }

        public TransactionCategory Category { get; set; }

        public string TransactionInfo {get; set;} = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
