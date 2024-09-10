namespace BulkTransactionServiceWebApi.Domain;

// FIXME: why are you using this class? Seems like a clone of the Presentation.BulkTransferRequest one.
// I believe you can achieve the goal by using 2 classes as DTOs and 2 classes for entities (domain)
public sealed class BulkTransferRequestDomain
{
    public string? OrganizationName { get; set; }

    public string? OrganizationBic { get; set; }

    public string? OrganizationIban { get; set; }

    public List<CreditTransferDomain>? CreditTransfers { get; set; }
}
