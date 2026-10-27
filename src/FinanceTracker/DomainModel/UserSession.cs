namespace DomainModel;

public class UserSession
{
    public Guid SessionKey { get; set; }

    public int AccountId { get; set; }

    public DateTime CreatedAt { get; set; }

    public UserSession(Guid sessionKey, int accountId, DateTime createdAt)
    {
        SessionKey = sessionKey;
        AccountId = accountId;
        CreatedAt = createdAt;
    }
}