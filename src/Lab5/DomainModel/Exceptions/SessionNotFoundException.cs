namespace DomainModel.Exceptions;

public class SessionNotFoundException : Exception
{
    public SessionNotFoundException(string message = "Сессия не найдена") : base(message) { }
}