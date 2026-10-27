using Application.Repositories;
using DomainModel;

namespace Infrastructure.InMemoryRepositories;

public class InMemorySessionRepository : ISessionRepository
{
    private readonly Dictionary<Guid, UserSession> _sessions = new();

    public Task<UserSession?> GetBySessionKey(Guid sessionKey)
    {
        _sessions.TryGetValue(sessionKey, out UserSession? session);
        return Task.FromResult(session);
    }

    public Task Save(UserSession userSession)
    {
        _sessions[userSession.SessionKey] = userSession;
        return Task.CompletedTask;
    }
}

//Комментарий для коммита