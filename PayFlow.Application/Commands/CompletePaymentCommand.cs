using MediatR;

namespace PayFlow.Application.Commands;

public record ValidatePaymentCommand(
    string InternalReferenceNumber,
    decimal Amount,
    string PhoneNumber,
    string BillRefNumber
) : IRequest<bool>;

public record CompletePaymentCommand(
    string InternalReferenceNumber,
    string ExternalReferenceNumber,
    string ResultCode
) : IRequest<bool>;
