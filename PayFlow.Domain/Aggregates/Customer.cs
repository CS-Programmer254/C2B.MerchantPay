using PayFlow.Domain.Shared;

namespace PayFlow.Domain.Aggregates;

public class Customer : AggregateRoot<Guid>
{
    public string FullName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;

    protected Customer() { }

    private Customer(Guid id, string name, string phone) : base(id)
    {
        FullName = name;
        PhoneNumber = phone;
    }

    public static Customer Create(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("Customer name cannot be empty");
        if (string.IsNullOrWhiteSpace(phone))
            throw new BusinessRuleException("Customer phone cannot be empty");

        return new Customer(Guid.NewGuid(), name, phone);
    }
}
