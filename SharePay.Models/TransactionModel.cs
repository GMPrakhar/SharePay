using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class TransactionModel : BaseModel
    {
        public Guid FromUser { get; set; }

        public required ISet<Guid> ToUsers { get; set; }

        [JsonIgnore]
        public IDivisionStrategy TransactionDivisionStrategy { get; set; } = DivisionStrategy.Equal;

        public required Dictionary<Guid, double> DivisionStrategyPerUserUnit { get; set; }

        public decimal TotalAmount { get; set; }

        public IDictionary<Guid, decimal> PerUserAmount => this.TransactionDivisionStrategy.GetAmountPerUser(ToUsers, DivisionStrategyPerUserUnit, TotalAmount);

        public required string Description { get; set; }

        public TransactionCategory Category { get; set; }
    }
}
