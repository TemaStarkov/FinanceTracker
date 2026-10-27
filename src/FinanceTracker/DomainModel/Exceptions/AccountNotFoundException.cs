namespace DomainModel.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string message = "Счет не найден") : base(message) { }
}