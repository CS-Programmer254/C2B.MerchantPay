using MediatR;

namespace PayFlow.Domain.Shared;
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    string CorrelationId { get; }
    string EventType { get; }
}

public abstract record DomainEvent : IDomainEvent, INotification
{
    public virtual Guid EventId { get; init; } = Guid.NewGuid();
    public virtual DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public virtual string CorrelationId { get; init; } = Guid.NewGuid().ToString();

    public virtual string SourceMicroservice { get; init; } = "UnknownService";

    public string EventType => GetType().Name;
}
