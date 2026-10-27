namespace Presentation.Requests;

public class DepositRequest
{
    public Guid SessionKey { get; set; }

    public decimal Amount { get; set; }
}