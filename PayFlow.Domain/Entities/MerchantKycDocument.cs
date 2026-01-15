using PayFlow.Domain.Shared;

namespace PayFlow.Domain.Entities;

public class MerchantKycDocument : Entity<Guid>
{
    public Guid MerchantId { get; private set; }
    public string DocumentType { get; private set; }
    public string DocumentNumber { get; private set; }
    public bool IsVerified { get; private set; }

    protected MerchantKycDocument() { }

    public MerchantKycDocument(Guid id, Guid merchantId, string type, string number)
        : base(id)
    {
        MerchantId = merchantId;
        DocumentType = type;
        DocumentNumber = number;
    }

    public void Verify()
    {
        IsVerified = true;
    }
}
