using SharePay.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class TransactionBalancer
{
    public static List<BalancedTransactionModel> CalculateTransactions(IDictionary<Guid, decimal> userBalanceMap)
    {
        var transactions = new List<BalancedTransactionModel>();

        // Separate users into debtors and creditors
        var debtors = userBalanceMap.Where(kvp => kvp.Value < 0).Select(kvp => new User(kvp.Key, -kvp.Value)).ToList();
        var creditors = userBalanceMap.Where(kvp => kvp.Value > 0).Select(kvp => new User(kvp.Key, kvp.Value)).ToList();

        int i = 0, j = 0;

        // Match debtors to creditors
        while (i < debtors.Count && j < creditors.Count)
        {
            var debtor = debtors[i];
            var creditor = creditors[j];

            // Determine the transaction amount
            decimal amount = Math.Min(debtor.Balance, creditor.Balance);

            transactions.Add(new BalancedTransactionModel(creditor.Id, debtor.Id, amount));

            // Update balances
            debtor.Balance -= amount;
            creditor.Balance -= amount;

            // Move to the next debtor or creditor if their balance is settled
            if (debtor.Balance == 0) i++;
            if (creditor.Balance == 0) j++;
        }

        return transactions;
    }
}

public class User
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }

    public User(Guid Id, decimal balance)
    {
        this.Id = Id;
        Balance = balance;
    }
}

