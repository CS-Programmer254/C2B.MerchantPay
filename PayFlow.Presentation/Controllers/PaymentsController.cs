using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.Commands;
using PayFlow.Application.Queries;

namespace PayFlow.Presentation.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Pay(InitiatePaymentCommand command)
    {
        var paymentId = await _mediator.Send(command);
        return Ok(new { paymentId });
    }

    [HttpGet("{id}/status")]
    public async Task<IActionResult> Status(Guid id)
    {
        var status = await _mediator.Send(
            new GetPaymentStatusQuery(id));

        return Ok(status);
    }
}
