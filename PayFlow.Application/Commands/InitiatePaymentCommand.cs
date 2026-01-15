using MediatR;

namespace PayFlow.Application.Commands;

public record InitiatePaymentCommand(
    Guid CustomerId,
    Guid MerchantId,
    decimal Amount,
    string PhoneNumber
) : IRequest<Guid>;

