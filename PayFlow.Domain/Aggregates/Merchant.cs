using PayFlow.Domain.Entities;
using PayFlow.Domain.Shared;

namespace PayFlow.Domain.Aggregates;

public class Merchant : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string MerchantType { get; private set; } 
    public bool IsActive { get; private set; }

    private readonly List<MerchantKycDocument> _kycDocuments = new();
    public IReadOnlyCollection<MerchantKycDocument> KycDocuments => _kycDocuments;

    protected Merchant() { }

    public Merchant(Guid id, string name, string type) : base(id)
    {
        Name = name;
        MerchantType = type;
        IsActive = false;
    }

    public void VerifyKyc()
    {
        if (!_kycDocuments.Any(d => d.IsVerified))
            throw new BusinessRuleException("KYC not verified");

        IsActive = true;
        IncrementVersion();
    }
}
