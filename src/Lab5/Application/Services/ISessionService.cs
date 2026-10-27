namespace Application.Services;

public interface ISessionService
{
    Task<Guid> CreateUserSession(string accountNumber, int pinCode);

    Task<Guid> CreateAdminSession(string systemPassword);

    Task<bool> ValidateSession(Guid sessionKey);
}