using PayFlow.Domain.Enums;
using PayFlow.Domain.Events;
using PayFlow.Domain.Shared;
using PayFlow.Domain.ValueObjects;

namespace PayFlow.Domain.Aggregates;

public class Payment : AggregateRoot<Guid>
{
    public Guid CustomerId { get; private set; }
    public Guid MerchantId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? ExternalReference { get; private set; } 

    protected Payment() { }

    public Payment(Guid id, Guid customerId, Guid merchantId, Money amount)
        : base(id)
    {
        CustomerId = customerId;
        MerchantId = merchantId;
        Amount = amount;
        Status = PaymentStatus.Pending;
    }

    public void MarkCompleted(string externalReference)
    {
        if (Status != PaymentStatus.Pending)
            return;

        Status = PaymentStatus.Completed;
        ExternalReference = externalReference;
        AddDomainEvent(new PaymentCompletedEvent(Id));
        IncrementVersion();
    }

    public void MarkFailed()
    {
        Status = PaymentStatus.Failed;
        IncrementVersion();
    }
}