using Application.Repositories;
using DomainModel;
using DomainModel.Exceptions;

namespace Application.Services;

public class SessionService : ISessionService
{
    public IAccountRepository AccountRepository { get; }

    public ISessionRepository SessionRepository { get; }

    public string SystemPassword { get; }

    public SessionService(IAccountRepository accountRepository, ISessionRepository sessionRepository, string systemPassword)
    {
        AccountRepository = accountRepository;
        SessionRepository = sessionRepository;
        SystemPassword = systemPassword;
    }

    public async Task<Guid> CreateUserSession(string accountNumber, int pinCode)
    {
        Account? account = await AccountRepository.GetByAccountNumber(accountNumber);
        if (account == null)
        {
            throw new AccountNotFoundException();
        }

        if (account.PinCode != pinCode)
        {
            throw new InvalidPinCodeException();
        }

        var sessionKey = Guid.NewGuid();
        var session = new UserSession(sessionKey, account.Id, DateTime.UtcNow);
        await SessionRepository.Save(session);

        return sessionKey;
    }

    public async Task<Guid> CreateAdminSession(string systemPassword)
    {
        if (systemPassword != SystemPassword)
        {
            throw new InvalidPasswordException();
        }

        var sessionKey = Guid.NewGuid();
        var session = new UserSession(sessionKey, -1, DateTime.UtcNow);
        await SessionRepository.Save(session);

        return sessionKey;
    }

    public async Task<bool> ValidateSession(Guid sessionKey)
    {
        UserSession? session = await SessionRepository.GetBySessionKey(sessionKey);
        return session != null;
    }
}