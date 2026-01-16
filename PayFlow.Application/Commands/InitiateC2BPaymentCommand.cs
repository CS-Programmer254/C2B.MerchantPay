using MediatR;
using PayFlow.Application.DTOs;

namespace PayFlow.Application.Commands;

public record InitiateC2BPaymentCommand(
    string Amount,
    string PhoneNumber,
    string BillRef,
    C2BTransactionType Type,
    string ShortCode
) : IRequest<Guid>;
