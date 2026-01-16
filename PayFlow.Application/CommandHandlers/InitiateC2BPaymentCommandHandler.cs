using MediatR;
using PayFlow.Application.Commands;
using PayFlow.Application.Interfaces;
using PayFlow.Domain.Aggregates;
using PayFlow.Domain.Shared;
using PayFlow.Domain.ValueObjects;

namespace PayFlow.Application.CommandHandlers;

public sealed class InitiateC2BPaymentCommandHandler : IRequestHandler<InitiateC2BPaymentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IC2BSimulator _simulator;

    public InitiateC2BPaymentCommandHandler(IUnitOfWork unitOfWork, IC2BSimulator simulator)
    {
        _unitOfWork = unitOfWork;
        _simulator = simulator;
    }

    public async Task<Guid> Handle(InitiateC2BPaymentCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (customer == null)
            throw new BusinessRuleException($"Customer with phone {request.PhoneNumber} not found");

        var merchant = await _unitOfWork.Merchants.GetByShortCodeAsync(request.ShortCode, cancellationToken);
        if (merchant == null)
            throw new BusinessRuleException($"Merchant with ShortCode {request.ShortCode} not found");
        if (!merchant.IsActive)
            throw new BusinessRuleException($"Merchant {merchant.Name} is not active");

        var payment = Payment.Create(customer.Id, merchant.Id, new Money(request.Amount, "KES"));

        await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _simulator.SimulateAsync(
            request.PhoneNumber,
            request.Amount,
            payment.InternalReferenceNumber, 
            request.Type
        );

        return payment.Id;
    }
}
