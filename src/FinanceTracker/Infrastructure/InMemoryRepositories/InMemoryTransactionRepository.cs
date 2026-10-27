using Application.Repositories;
using DomainModel;

namespace Infrastructure.InMemoryRepositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    private int _nextId = 1;

    public Task<IEnumerable<Transaction>> GetByAccountId(int accountId)
    {
        var transactions = _transactions.Where(t => t.AccountId == accountId).ToList();
        return Task.FromResult((IEnumerable<Transaction>)transactions);
    }

    public Task Save(Transaction transaction)
    {
        if (transaction.Id == 0)
        {
            transaction.Id = _nextId++;
        }

        _transactions.Add(transaction);
        return Task.CompletedTask;
    }
}