using System.Text.Json.Serialization;

namespace BulkTransactionServiceWebApi.Presentation;

public sealed class CreditTransfer
{
    [JsonPropertyName("amount")]
    public string? Amount { get; set; }

    [JsonPropertyName("counterparty_name")]
    public string? CounterpartyName { get; set; }

    [JsonPropertyName("counterparty_bic")]
    public string? CounterpartyBic { get; set; }

    [JsonPropertyName("counterparty_iban")]
    public string? CounterpartyIban { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
