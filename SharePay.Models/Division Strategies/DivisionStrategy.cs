
using SharePay.Models.Division_Strategies;

namespace SharePay.Models
{
    public class DivisionStrategy : IDivisionStrategy
    {
        public static IDivisionStrategy Equal => new EqualDivisionStrategy(); 

        public virtual string Name => throw new NotImplementedException();

        public virtual string Description => throw new NotImplementedException();

        public virtual IDictionary<Guid, decimal> GetAmountPerUser(ISet<Guid> userIds, IDictionary<Guid, double> userDivisionUnitMap, decimal TotalAmount)
        {
            return userIds.ToDictionary(x => x, x => TotalAmount / userIds.Count);
        }
    }
}