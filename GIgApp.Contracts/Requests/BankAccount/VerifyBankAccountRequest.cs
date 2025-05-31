namespace GigApp.Contracts.Requests.BankAccount;

public class VerifyBankAccountRequest
{
    public int Id { get; set; }
    public List<long?> Amounts { get; set; }
}