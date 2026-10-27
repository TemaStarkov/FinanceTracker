namespace DomainModel.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string message = "Неверный системный пароль") : base(message) { }
}