using PayFlow.Domain.Enums;
using PayFlow.Domain.Events;
using PayFlow.Domain.Shared;
using PayFlow.Domain.ValueObjects;

namespace PayFlow.Domain.Aggregates;

public class Payment : AggregateRoot<Guid>
{
    public Guid CustomerId { get; private set; }
    public Guid MerchantId { get; private set; }
    public Money Amount { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }

    public string InternalReferenceNumber { get; private set; } = null!;
    public string? ExternalReference { get; private set; }

    protected Payment() { }

    private Payment(Guid id, Guid customerId, Guid merchantId, Money amount, string internalReference)
        : base(id)
    {
        CustomerId = customerId;
        MerchantId = merchantId;
        Amount = amount;
        Status = PaymentStatus.Pending;
        InternalReferenceNumber = internalReference;
    }


    public static Payment Create(Guid customerId, Guid merchantId, Money amount)
    {
        var id = Guid.NewGuid();
        var internalReference = GenerateInternalReference();
        return new Payment(id, customerId, merchantId, amount, internalReference);
    }

    /// <summary>Generates a reference like PAYFLOW-HKSGHK</summary>
    private static string GenerateInternalReference()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        var letters = new string(Enumerable.Range(0, 6)
            .Select(_ => chars[random.Next(chars.Length)]).ToArray());
        return $"PAYFLOW-{letters}";
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
