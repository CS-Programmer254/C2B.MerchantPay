using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.Commands;
using PayFlow.Application.DTOs;
using PayFlow.Application.Queries;

namespace PayFlow.Presentation.Controllers;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initiate-c2b")]
    public async Task<IActionResult> InitiateC2B([FromBody] C2BSimulateRequest request)
    {
        if (!Enum.TryParse<C2BTransactionType>(request.C2BTransactionType, true, out var type))
        {
            return BadRequest($"Invalid C2BTransactionType: {request.C2BTransactionType}");
        }

        var paymentId = await _mediator.Send(
            new InitiateC2BPaymentCommand(
                Amount: request.Amount,
                PhoneNumber: request.Phone,
                BillRef: request.BillRef,
                Type: type,
                ShortCode: request.ShortCode
            )
        );

        return Ok(new
        {
            paymentId,
            message = $"C2B Payment Initiated to {type}-{request.BillRef}"
        });
    }

    [HttpGet("{id}/status")]
    public async Task<IActionResult> Status(Guid id)
    {
        var status = await _mediator.Send(new GetPaymentStatusQuery(id));
        return Ok(status);
    }
}
