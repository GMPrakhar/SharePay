using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class BalancedTransactionModel
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public decimal Amount { get; set; }

        public BalancedTransactionModel(Guid from, Guid to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
