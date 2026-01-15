using MediatR;
using PayFlow.Application.Commands;
using PayFlow.Application.Interfaces;
using PayFlow.Domain.Aggregates;
using PayFlow.Domain.ValueObjects;

namespace PayFlow.Application.CommandHandlers;

public sealed class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMobileMoneyGateway _gateway;

    public InitiatePaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IMobileMoneyGateway gateway)
    {
        _unitOfWork = unitOfWork;
        _gateway = gateway;
    }

    public async Task<Guid> Handle(
        InitiatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = new Payment(
            Guid.NewGuid(),
            request.CustomerId,
            request.MerchantId,
            new Money(request.Amount, "KES")
        );

        await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _gateway.InitiateAsync(
            request.PhoneNumber,
            request.Amount,
            payment.Id.ToString()
        );

        return payment.Id;
    }
}