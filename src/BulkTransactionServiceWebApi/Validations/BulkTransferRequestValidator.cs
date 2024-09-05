using BulkTransactionServiceWebApi.Presentation;
using FluentValidation;

namespace BulkTransactionServiceWebApi.Validations;

public sealed class BulkTransferRequestValidator : AbstractValidator<BulkTransferRequest>
{
    public BulkTransferRequestValidator()
    {
        RuleFor(x => x.OrganizationName).NotEmpty();
        RuleFor(x => x.OrganizationIban).NotEmpty();
        RuleFor(x => x.OrganizationBic).NotEmpty();
        RuleFor(x => x.CreditTransfers).NotEmpty();
        RuleForEach(x => x.CreditTransfers).SetValidator(new CreditTransferValidator());
    }
}
