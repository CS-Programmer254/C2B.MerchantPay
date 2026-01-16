using MediatR;
using PayFlow.Application.Commands;
using PayFlow.Application.Interfaces;

namespace PayFlow.Application.CommandHandlers;

public sealed class CompletePaymentCommandHandler : IRequestHandler<CompletePaymentCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CompletePaymentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments
            .GetByInternalReferenceAsync(request.InternalReferenceNumber, cancellationToken);

        if (payment == null)
            throw new Exception($"Payment with reference {request.InternalReferenceNumber} not found");

        if (request.ResultCode == "0")
            payment.MarkCompleted(request.ExternalReferenceNumber);
        else
            payment.MarkFailed();

        // Update wallets
        if (payment.Status == Domain.Enums.PaymentStatus.Completed)
        {
            var customerWallet = await _unitOfWork.Wallets.GetByOwnerIdAsync(payment.CustomerId, cancellationToken);
            var merchantWallet = await _unitOfWork.Wallets.GetByOwnerIdAsync(payment.MerchantId, cancellationToken);

            if (customerWallet == null || merchantWallet == null)
                throw new Exception("Wallets not found");

            customerWallet.Debit(payment.Amount);
            merchantWallet.Credit(payment.Amount);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
