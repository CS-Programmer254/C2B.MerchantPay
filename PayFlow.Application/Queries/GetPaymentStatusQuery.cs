using MediatR;
using PayFlow.Application.DTOs;

namespace PayFlow.Application.Queries;

public sealed record GetPaymentStatusQuery(Guid PaymentId)
    : IRequest<PaymentStatusDto>;
