namespace Presentation.Requests;

public class WithdrawRequest
{
    public Guid SessionKey { get; set; }

    public decimal Amount { get; set; }
}