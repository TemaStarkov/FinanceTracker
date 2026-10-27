using DomainModel;

namespace Application.Repositories;

public interface ISessionRepository
{
    Task<UserSession?> GetBySessionKey(Guid sessionKey);

    Task Save(UserSession userSession);
}