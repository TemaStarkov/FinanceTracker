using DomainModel;

namespace Application.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetByAccountId(int accountId);

    Task Save(Transaction transaction);
}