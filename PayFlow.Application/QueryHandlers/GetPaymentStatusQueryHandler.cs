using MediatR;
using PayFlow.Application.DTOs;
using PayFlow.Application.Interfaces;
using PayFlow.Application.Queries;
using PayFlow.Domain.Aggregates;
using PayFlow.Domain.Shared;

namespace PayFlow.Application.QueryHandlers;

public sealed class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, PaymentStatusDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPaymentStatusQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentStatusDto> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(request.PaymentId, cancellationToken);

        if (payment == null)
        {
            throw new NotFoundException(nameof(Payment), request.PaymentId);
        }

        return new PaymentStatusDto
        {
            PaymentId = payment.Id,
            Status = payment.Status.ToString(),
            Amount = payment.Amount.Amount,
            Currency = payment.Amount.Currency,
            CreatedAt = payment.CreatedAt
        };
    }
}