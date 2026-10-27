using Application.Repositories;
using DomainModel;
using DomainModel.Exceptions;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ISessionRepository sessionRepository, ITransactionRepository transactionRepository)
    {
        _sessionRepository = sessionRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionHistory(Guid sessionKey)
    {
        UserSession? session = await _sessionRepository.GetBySessionKey(sessionKey);
        if (session == null)
        {
            throw new SessionNotFoundException();
        }

        IEnumerable<Transaction> transactions = await _transactionRepository.GetByAccountId(session.AccountId);
        return transactions;
    }
}