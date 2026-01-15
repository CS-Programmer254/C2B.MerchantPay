using PayFlow.Domain.Shared;
using PayFlow.Domain.ValueObjects;

namespace PayFlow.Domain.Aggregates;

public class Wallet : AggregateRoot<Guid>
{
    public Guid OwnerId { get; private set; }
    public string OwnerType { get; private set; } 
    public WalletBalance Balance { get; private set; }

    protected Wallet() { }

    public Wallet(Guid id, Guid ownerId, string ownerType, Money openingBalance)
        : base(id)
    {
        OwnerId = ownerId;
        OwnerType = ownerType;
        Balance = new WalletBalance(openingBalance);
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
