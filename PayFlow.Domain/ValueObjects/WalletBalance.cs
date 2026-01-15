using PayFlow.Domain.Shared;

namespace PayFlow.Domain.ValueObjects;

public sealed class WalletBalance : ValueObject
{
    public Money Value { get; }

    public WalletBalance(Money value)
    {
        Value = value;
    }

    public WalletBalance Credit(Money amount)
        => new(new Money(Value.Amount + amount.Amount, Value.Currency));

    public WalletBalance Debit(Money amount)
    {
        if (Value.Amount < amount.Amount)
            throw new BusinessRuleException("Insufficient funds");

        return new(new Money(Value.Amount - amount.Amount, Value.Currency));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

