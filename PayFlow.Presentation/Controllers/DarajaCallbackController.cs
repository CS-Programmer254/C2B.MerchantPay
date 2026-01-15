using Microsoft.AspNetCore.Mvc;
using PayFlow.Application.DTOs;
using PayFlow.Application.Interfaces;
using PayFlow.Domain.Aggregates;
using PayFlow.Domain.Shared;

namespace PayFlow.Presentation.Controllers;

[ApiController]
[Route("api/payments/callback")]
public class DarajaCallbackController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DarajaCallbackController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> Callback(DarajaCallbackDto dto)
    {
        if (!Guid.TryParse(dto.AccountReference, out var paymentId))
        {
            return BadRequest("Invalid payment reference");
        }

        var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);

        //if (payment == null)
        //{
        //    throw new NotFoundException(nameof(Payment), paymentId);
        //}

        if (dto.ResultCode == "0")
        {
            payment.MarkCompleted(dto.CheckoutRequestID);
        }
        else
        {
            payment.MarkFailed();
        }

        await _unitOfWork.SaveChangesAsync();

        return Ok();
    }
}