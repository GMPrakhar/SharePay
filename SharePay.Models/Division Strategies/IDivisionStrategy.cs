namespace SharePay.Models
{
    public interface IDivisionStrategy
    {
        string Name { get; }
        string Description { get; }

        public IDictionary<Guid, decimal> GetAmountPerUser(ISet<Guid> userIds, IDictionary<Guid, double> userDivisionUnitMap, decimal TotalAmount);
    }
}