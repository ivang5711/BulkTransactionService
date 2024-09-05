using BulkTransactionServiceWebApi.Domain;
using BulkTransactionServiceWebApi.Presentation;
using BulkTransactionServiceWebApi.Services;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BulkTransactionServiceWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SalariesController(PaymentService salaryService) : ControllerBase
{
    private readonly PaymentService _salaryService = salaryService;

    [HttpPost]
    public async Task<ActionResult> PaySalaryAsync(
        BulkTransferRequest request,
        [FromServices] IValidator<BulkTransferRequest> validator
    )
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity();
        }

        var salaryPaymentRequest = request.Adapt<BulkTransferRequestDomain>();
        var result = await _salaryService.PaySalaryAsync(salaryPaymentRequest);
        if (result)
        {
            return StatusCode(StatusCodes.Status201Created);
        }

        return UnprocessableEntity();
    }
}
