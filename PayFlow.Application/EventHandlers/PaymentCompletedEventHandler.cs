using MediatR;
using PayFlow.Application.Interfaces;
using PayFlow.Domain.Aggregates;
using PayFlow.Domain.Events;
using PayFlow.Domain.Shared;

namespace PayFlow.Application.EventHandlers;
public sealed class PaymentCompletedEventHandler : INotificationHandler<PaymentCompletedEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentCompletedEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PaymentCompletedEvent notification, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments.GetByIdAsync(notification.PaymentId, cancellationToken);

        if (payment == null)
        {
            throw new NotFoundException(nameof(Payment), notification.PaymentId);
        }

        var customerWallet = await _unitOfWork.Wallets.GetByOwnerIdAsync(payment.CustomerId, cancellationToken);

        if (customerWallet == null)
        {
            throw new NotFoundException("Customer Wallet", payment.CustomerId);
        }

        var merchantWallet = await _unitOfWork.Wallets.GetByOwnerIdAsync(payment.MerchantId, cancellationToken);

        if (merchantWallet == null)
        {
            throw new NotFoundException("Merchant Wallet", payment.MerchantId);
        }

        customerWallet.Debit(payment.Amount);
        merchantWallet.Credit(payment.Amount);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}