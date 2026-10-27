using Application.Repositories;
using DomainModel;
using DomainModel.Exceptions;

namespace Application.Services;

public class AccountService : IAccountService
{
    public IAccountRepository AccountRepository { get; }

    public ISessionRepository SessionRepository { get; }

    public ITransactionRepository TransactionRepository { get; }

    public AccountService(
        IAccountRepository accountRepository,
        ISessionRepository sessionRepository,
        ITransactionRepository transactionRepository)
    {
        AccountRepository = accountRepository;
        SessionRepository = sessionRepository;
        TransactionRepository = transactionRepository;
    }

    public async Task<bool> Withdraw(Guid sessionKey, decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException();
        }

        UserSession? session = await SessionRepository.GetBySessionKey(sessionKey);
        if (session == null)
        {
            throw new SessionNotFoundException();
        }

        Account? account = await AccountRepository.GetById(session.AccountId);
        if (account == null)
        {
            throw new AccountNotFoundException();
        }

        if (amount > account.Balance)
        {
            throw new InsufficientFundsException();
        }

        account.Balance -= amount;
        await AccountRepository.Save(account);

        var transaction = new Transaction(0, account.Id, amount, TransactionType.Withdrawal, DateTime.UtcNow);
        await TransactionRepository.Save(transaction);

        return true;
    }

    public async Task<bool> Deposit(Guid sessionKey, decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidAmountException();
        }

        UserSession? session = await SessionRepository.GetBySessionKey(sessionKey);
        if (session == null)
        {
            throw new SessionNotFoundException();
        }

        Account? account = await AccountRepository.GetById(session.AccountId);
        if (account == null)
        {
            throw new AccountNotFoundException();
        }

        account.Balance += amount;
        await AccountRepository.Save(account);

        var transaction = new Transaction(0, account.Id, amount, TransactionType.Deposit, DateTime.UtcNow);
        await TransactionRepository.Save(transaction);

        return true;
    }

    public async Task<decimal> GetBalance(Guid sessionKey)
    {
        UserSession? session = await SessionRepository.GetBySessionKey(sessionKey);
        if (session == null)
        {
            throw new SessionNotFoundException();
        }

        Account? account = await AccountRepository.GetById(session.AccountId);
        if (account == null)
        {
            throw new AccountNotFoundException();
        }

        return account.Balance;
    }
}