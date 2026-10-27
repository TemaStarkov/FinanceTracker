namespace DomainModel;

public class Account
{
    public int Id { get; set; }

    public string AccountNumber { get; set; }

    public decimal Balance { get; set; }

    public int PinCode { get; set; }

    public Account(int id, string accountNumber, decimal balance, int pinCode)
    {
        Id = id;
        AccountNumber = accountNumber;
        Balance = balance;
        PinCode = pinCode;
    }
}