namespace DomainModel;

public class Transaction
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public DateTime Timestamp { get; set; }

    public Transaction(int id, int accountId, decimal amount, TransactionType type, DateTime timestamp)
    {
        Id = id;
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Timestamp = timestamp;
    }
}
