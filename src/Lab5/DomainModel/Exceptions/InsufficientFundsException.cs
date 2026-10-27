namespace DomainModel.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message = "Недостаточно средств на счете") : base(message) { }
}