namespace Presentation.Requests;

public class CreateUserSessionRequest
{
    public string AccountNumber { get; set; } = string.Empty;

    public int PinCode { get; set; }
}