using Application.Repositories;
using Application.Services;
using DomainModel;
using DomainModel.Exceptions;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public class AccountServiceTests
{
    [Fact]
    public async Task Withdraw_WithSufficientBalance_UpdatesAccountBalance()
    {
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        ITransactionRepository transactionRepository = Substitute.For<ITransactionRepository>();

        var testAccount = new Account(1, "123456", 1000, 1234);
        var testSession = new UserSession(Guid.NewGuid(), 1, DateTime.UtcNow);

        sessionRepository.GetBySessionKey(testSession.SessionKey).Returns(Task.FromResult<UserSession?>(testSession));
        accountRepository.GetById(1).Returns(Task.FromResult<Account?>(testAccount));

        var service = new AccountService(accountRepository, sessionRepository, transactionRepository);

        await service.Withdraw(testSession.SessionKey, 100);

        Assert.Equal(900, testAccount.Balance);
        await accountRepository.Received(1).Save(testAccount);
        await transactionRepository.Received(1).Save(Arg.Any<Transaction>());
    }

    [Fact]
    public async Task Withdraw_WithInsufficientBalance_ThrowsException()
    {
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        ITransactionRepository transactionRepository = Substitute.For<ITransactionRepository>();

        var testAccount = new Account(1, "123456", 100, 1234);
        var testSession = new UserSession(Guid.NewGuid(), 1, DateTime.UtcNow);

        sessionRepository.GetBySessionKey(testSession.SessionKey).Returns(Task.FromResult<UserSession?>(testSession));
        accountRepository.GetById(1).Returns(Task.FromResult<Account?>(testAccount));
        var service = new AccountService(accountRepository, sessionRepository, transactionRepository);

        await Assert.ThrowsAsync<InsufficientFundsException>(
            () => service.Withdraw(testSession.SessionKey, 200));
    }

    [Fact]
    public async Task Deposit_WithValidAmount_UpdatesAccountBalance()
    {
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        ITransactionRepository transactionRepository = Substitute.For<ITransactionRepository>();

        var testAccount = new Account(1, "123456", 1000, 1234);
        var testSession = new UserSession(Guid.NewGuid(), 1, DateTime.UtcNow);

        sessionRepository.GetBySessionKey(testSession.SessionKey).Returns(Task.FromResult<UserSession?>(testSession));
        accountRepository.GetById(1).Returns(Task.FromResult<Account?>(testAccount));

        var service = new AccountService(accountRepository, sessionRepository, transactionRepository);
        await service.Deposit(testSession.SessionKey, 500);

        Assert.Equal(1500, testAccount.Balance);
        await accountRepository.Received(1).Save(testAccount);
        await transactionRepository.Received(1).Save(Arg.Any<Transaction>());
    }

    [Fact]
    public async Task Withdraw_CreatesTransaction()
    {
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        ITransactionRepository transactionRepository = Substitute.For<ITransactionRepository>();

        var testAccount = new Account(1, "123456", 1000, 1234);
        var testSession = new UserSession(Guid.NewGuid(), 1, DateTime.UtcNow);

        sessionRepository.GetBySessionKey(testSession.SessionKey).Returns(Task.FromResult<UserSession?>(testSession));
        accountRepository.GetById(1).Returns(Task.FromResult<Account?>(testAccount));

        var service = new AccountService(accountRepository, sessionRepository, transactionRepository);

        await service.Withdraw(testSession.SessionKey, 100);

        await transactionRepository.Received(1).Save(Arg.Is<Transaction>(t =>
            t.AccountId == 1 &&
            t.Amount == 100 &&
            t.Type == TransactionType.Withdrawal));
    }

    [Fact]
    public async Task Deposit_CreatesTransaction()
    {
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        ITransactionRepository transactionRepository = Substitute.For<ITransactionRepository>();

        var testAccount = new Account(1, "123456", 1000, 1234);
        var testSession = new UserSession(Guid.NewGuid(), 1, DateTime.UtcNow);

        sessionRepository.GetBySessionKey(testSession.SessionKey).Returns(Task.FromResult<UserSession?>(testSession));
        accountRepository.GetById(1).Returns(Task.FromResult<Account?>(testAccount));
        var service = new AccountService(accountRepository, sessionRepository, transactionRepository);
        await service.Deposit(testSession.SessionKey, 500);

        await transactionRepository.Received(1).Save(Arg.Is<Transaction>(t =>
            t.AccountId == 1 &&
            t.Amount == 500 &&
            t.Type == TransactionType.Deposit));
    }
}
