namespace BulkTransactionServiceWebApi.Domain;

public sealed class CreditTransferDomain
{
    public string? Amount { get; set; }

    public int AmountDigit { get; set; }

    public string? CounterpartyName { get; set; }

    public string? CounterpartyBic { get; set; }

    public string? CounterpartyIban { get; set; }

    public string? Description { get; set; }

    public int BankAccountId { get; set; }
}
