using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharePay.Models
{
    public class TransactionModel : BaseModel
    {
        [JsonPropertyName("from_user")]
        public Guid FromUser { get; set; }

        [JsonPropertyName("to_users")]

        public required ISet<Guid> ToUsers { get; set; }

        [JsonIgnore]
        public IDivisionStrategy TransactionDivisionStrategy { get; set; } = DivisionStrategy.Equal;

        [JsonPropertyName("division_strategy_per_user_unit")]
        public required Dictionary<Guid, double> DivisionStrategyPerUserUnit { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }

        public IDictionary<Guid, decimal> PerUserAmount => this.TransactionDivisionStrategy.GetAmountPerUser(ToUsers, DivisionStrategyPerUserUnit, TotalAmount);

        public required string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionCategory Category { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
