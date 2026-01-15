using PayFlow.Domain.Shared;
namespace PayFlow.Domain.Events;

public sealed record PaymentCompletedEvent(Guid PaymentId) : DomainEvent;
