using System.Text.Json.Serialization;

namespace BulkTransactionServiceWebApi.Presentation;
public sealed class BulkTransferRequest
{
    [JsonPropertyName("organization_name")]
    public string? OrganizationName { get; set; }

    [JsonPropertyName("organization_bic")]
    public string? OrganizationBic { get; set; }

    [JsonPropertyName("organization_iban")]
    public string? OrganizationIban { get; set; }

    [JsonPropertyName("credit_transfers")]
    public CreditTransfer[]? CreditTransfers { get; set; }
}
