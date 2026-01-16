using PayFlow.Domain.Shared;

namespace PayFlow.Domain.ValueObjects;

public sealed class WalletBalance : ValueObject
{
    public Money Value { get; private set; } = null!;

    // Parameterless constructor for EF Core
    protected WalletBalance() { }

    public WalletBalance(Money value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public WalletBalance Credit(Money amount)
        => new WalletBalance(new Money(Value.Amount + amount.Amount, Value.Currency));

    public WalletBalance Debit(Money amount)
    {
        if (Value.Amount < amount.Amount)
            throw new BusinessRuleException("Insufficient funds");

        return new WalletBalance(new Money(Value.Amount - amount.Amount, Value.Currency));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
