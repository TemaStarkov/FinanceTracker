namespace DomainModel.Exceptions;

public class InvalidPinCodeException : Exception
{
    public InvalidPinCodeException(string message = "Неверный PIN-код") : base(message) { }
}