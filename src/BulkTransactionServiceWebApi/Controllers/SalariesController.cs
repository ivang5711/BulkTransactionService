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

    // FIXME: the endpoint address is not good. Should be something like /api/customer-transfers
    // Keep in mind that we're tracking transfers. They might be something else, not salaries.
    // have a look at the REST standard best practices
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
        // FIXME: check the bad case, not the good one. The line "return StatusCode(StatusCodes.Status201Created);" should be the last one of the function.
        if (result)
        {
            // FIXME: when you return 201 Created, you should return the id of the resource you've just created.
            // for a bulk operation, it's better to return 202 accepted and return the id to track the whole request.
            return StatusCode(StatusCodes.Status201Created);
        }

        // FIXME: the response content is not good.
        // {
        //     "type": "https://tools.ietf.org/html/rfc4918#section-11.2",
        //     "title": "Unprocessable Entity",
        //     "status": 422,
        //     "traceId": "00-32ab861abbecc1ed83b049a6017e7d54-f9cea5401d0e4889-00"
        // }
        // you're duplicating info: "title", "status" can be already seen by the response status code
        // "type" is not relevant: we assume anyone knows what 422 means
        // "traceId" has some meaning? Or is only a random GUID?
        // The client must be aware of the reason of this failure (clearly states we don't have enough money to perform the operation)
        return UnprocessableEntity();
    }
}
