using DomainModel;

namespace Application.Services;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetTransactionHistory(Guid sessionKey);
}
