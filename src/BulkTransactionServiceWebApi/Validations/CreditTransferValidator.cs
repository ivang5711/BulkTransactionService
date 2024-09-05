using BulkTransactionServiceWebApi.Presentation;
using FluentValidation;

namespace BulkTransactionServiceWebApi.Validations;

public sealed class CreditTransferValidator : AbstractValidator<CreditTransfer>
{
    public CreditTransferValidator()
    {
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.CounterpartyBic).NotEmpty();
        RuleFor(x => x.CounterpartyIban).NotEmpty();
        RuleFor(x => x.CounterpartyName).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
