using PayFlow.Domain.Shared;

namespace PayFlow.Domain.Aggregates;

public class Customer : AggregateRoot<Guid>
{
    public string FullName { get; private set; }
    public string PhoneNumber { get; private set; }

    protected Customer() { }

    public Customer(Guid id, string name, string phone) : base(id)
    {
        FullName = name;
        PhoneNumber = phone;
    }
}

