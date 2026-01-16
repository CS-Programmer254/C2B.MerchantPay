using MediatR;
using PayFlow.Application.Commands;
using PayFlow.Application.Interfaces;

namespace PayFlow.Application.CommandHandlers;

public sealed class ValidatePaymentCommandHandler : IRequestHandler<ValidatePaymentCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidatePaymentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ValidatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments
            .GetByInternalReferenceAsync(request.InternalReferenceNumber, cancellationToken);

        if (payment == null || payment.Status != Domain.Enums.PaymentStatus.Pending)
            return false; // Reject if not found or already processed

        //validate amount matches
        if (payment.Amount.Amount != request.Amount)
            return false;

        return true; // Accept payment
    }
}
