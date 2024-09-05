namespace BulkTransactionServiceWebApi.Domain;

public class BankAccount
{
    public int Id { get; set; }
    public required string OrganizationName { get; set; }
    public int BalanceCents { get; set; }
    public required string Iban { get; set; }
    public required string Bic { get; set; }
}