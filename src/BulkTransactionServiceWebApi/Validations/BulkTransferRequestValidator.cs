using BulkTransactionServiceWebApi.Presentation;

using FluentValidation;

namespace BulkTransactionServiceWebApi.Validations;

public sealed class BulkTransferRequestValidator : AbstractValidator<BulkTransferRequest>
{
    public BulkTransferRequestValidator()
    {
        RuleFor(x => x.OrganizationName).NotEmpty();
        // TODO: handle not valid IBAN (basic validation is enough)
        RuleFor(x => x.OrganizationIban).NotEmpty();
        // TODO: handle not valid BIC (basic validation is enough)
        RuleFor(x => x.OrganizationBic).NotEmpty();
        RuleFor(x => x.CreditTransfers).NotEmpty();
        RuleForEach(x => x.CreditTransfers).SetValidator(new CreditTransferValidator());
    }
}
