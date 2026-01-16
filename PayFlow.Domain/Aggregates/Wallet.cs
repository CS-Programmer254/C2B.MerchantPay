using PayFlow.Domain.Shared;
using PayFlow.Domain.ValueObjects;

namespace PayFlow.Domain.Aggregates;

public class Wallet : AggregateRoot<Guid>
{
    public Guid OwnerId { get; private set; }
    public string OwnerType { get; private set; } = null!;
    public WalletBalance Balance { get; private set; } = null!;

    protected Wallet() { } // EF

    private Wallet(Guid id, Guid ownerId, string ownerType, Money openingBalance)
        : base(id)
    {
        OwnerId = ownerId;
        OwnerType = ownerType;
        Balance = new WalletBalance(openingBalance);
    }

    public static Wallet Create(Guid ownerId, string ownerType, Money openingBalance)
    {
        if (ownerId == Guid.Empty)
            throw new BusinessRuleException("OwnerId is required");

        if (string.IsNullOrWhiteSpace(ownerType))
            throw new BusinessRuleException("OwnerType is required");

        return new Wallet(
            Guid.NewGuid(),
            ownerId,
            ownerType,
            openingBalance
        );
    }

    public void Credit(Money amount)
    {
        Balance = Balance.Credit(amount);
        IncrementVersion();
    }

    public void Debit(Money amount)
    {
        Balance = Balance.Debit(amount);
        IncrementVersion();
    }
}
