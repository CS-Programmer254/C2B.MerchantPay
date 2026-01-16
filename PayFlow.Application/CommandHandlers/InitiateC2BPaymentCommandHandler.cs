using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<InitiateC2BPaymentCommandHandler>? _logger;

    public InitiateC2BPaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IC2BSimulator simulator,
        ILogger<InitiateC2BPaymentCommandHandler>? logger = null)
    {
        _unitOfWork = unitOfWork;
        _simulator = simulator;
        _logger = logger;
    }

    public async Task<Guid> Handle(InitiateC2BPaymentCommand request, CancellationToken cancellationToken)
    {
        _logger?.LogInformation("Initiating C2B Payment - Phone: {Phone}, Amount: {Amount}", request.PhoneNumber, request.Amount);

        var customer = await _unitOfWork.Customers.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (customer == null)
            throw new BusinessRuleException($"Customer with phone {request.PhoneNumber} not found");

        var merchant = await _unitOfWork.Merchants.GetByShortCodeAsync(request.ShortCode, cancellationToken);
        if (merchant == null)
            throw new BusinessRuleException($"Merchant with ShortCode {request.ShortCode} not found");
        if (!merchant.IsActive)
            throw new BusinessRuleException($"Merchant {merchant.Name} is not active");
      
        await _simulator.SimulateAsync(
            request.PhoneNumber,
            request.Amount,
            request.BillRef,
            request.Type
        );

        //Parse the amount string to decimal
        if (!decimal.TryParse(request.Amount.ToString(), out var amountDecimal))
        {
            throw new BusinessRuleException($"Invalid amount: {request.Amount}");
        }

        // Only save payment after successful simulation

        _logger?.LogInformation("Simulation successful, creating payment record");
        var payment = Payment.Create(customer.Id, merchant.Id, new Money(amountDecimal, "KES"));

        await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger?.LogInformation("Payment record saved - PaymentId: {PaymentId}", payment.Id);

        return payment.Id;
    }
}
