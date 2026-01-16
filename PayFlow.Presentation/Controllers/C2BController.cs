using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.Commands;
using PayFlow.Application.DTOs;

namespace PayFlow.Presentation.Controllers;

[ApiController]
[Route("api/c2b")]
public sealed class C2BController : ControllerBase
{
    private readonly IMediator _mediator;

    public C2BController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("validation")]
    public async Task<IActionResult> Validate([FromBody] C2BValidationRequest request)
    {
        var isValid = await _mediator.Send(new ValidatePaymentCommand(
            request.BillRefNumber, 
            request.TransAmount,
            request.MSISDN,
            request.BillRefNumber
        ));

        var response = new C2BValidationResponse(
            ResultCode: isValid ? "0" : "C2B00013",
            ResultDesc: isValid ? "Accepted" : "Rejected"
        );

        return Ok(response);
    }

    [HttpPost("confirmation")]
    public async Task<IActionResult> Confirm([FromBody] C2BConfirmationRequest request)
    {
        await _mediator.Send(new CompletePaymentCommand(
            InternalReferenceNumber: request.BillRefNumber,
            ExternalReferenceNumber: request.TransID,
            ResultCode: "0"
        ));

        var response = new C2BConfirmationResponse(
            ResultCode: "0",
            ResultDesc: "Accepted"
        );

        return Ok(response);
    }
}
