using Application.Repositories;
using DomainModel;

namespace Infrastructure.InMemoryRepositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<int, Account> _accounts = new();

    private int _nextId = 1;

    public Task<Account?> GetById(int id)
    {
        _accounts.TryGetValue(id, out Account? account);
        return Task.FromResult(account);
    }

    public Task<Account?> GetByAccountNumber(string accountNumber)
    {
        Account? account = _accounts.Values.FirstOrDefault(a => a.AccountNumber == accountNumber);
        return Task.FromResult(account);
    }

    public Task Save(Account account)
    {
        if (account.Id == 0)
        {
            account.Id = _nextId++;
        }

        _accounts[account.Id] = account;
        return Task.CompletedTask;
    }
}