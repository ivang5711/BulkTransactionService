namespace BulkTransactionServiceWebApi.Domain;

public sealed class BulkTransferRequestDomain
{
    public string? OrganizationName { get; set; }

    public string? OrganizationBic { get; set; }

    public string? OrganizationIban { get; set; }

    public List<CreditTransferDomain>? CreditTransfers { get; set; }
}
