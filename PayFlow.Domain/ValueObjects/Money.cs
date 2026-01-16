using PayFlow.Domain.Shared;

namespace PayFlow.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    // EF Core needs public getter + private setter
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;

    // Parameterless constructor for EF Core
    protected Money() { }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new BusinessRuleException("Money cannot be negative");

        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
