using DomainModel;

namespace Application.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetById(int id);

    Task<Account?> GetByAccountNumber(string accountNumber);

    Task Save(Account account);
}