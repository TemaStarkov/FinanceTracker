namespace DomainModel.Exceptions;

public class InvalidAmountException : Exception
{
    public InvalidAmountException(string message = "Сумма должна быть положительной") : base(message) { }
}