namespace Application.Services;

public interface IAccountService
{
    Task<bool> Withdraw(Guid sessionKey, decimal amount);

    Task<bool> Deposit(Guid sessionKey, decimal amount);

    Task<decimal> GetBalance(Guid sessionKey);
}