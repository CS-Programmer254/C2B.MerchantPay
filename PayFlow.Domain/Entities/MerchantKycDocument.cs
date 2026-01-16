using PayFlow.Domain.Shared;

namespace PayFlow.Domain.Entities;

public class MerchantKycDocument : Entity<Guid>
{
    public Guid MerchantId { get; private set; }
    public string DocumentType { get; private set; } = null!;
    public string DocumentNumber { get; private set; } = null!;
    public bool IsVerified { get; private set; }

    protected MerchantKycDocument() { }

    public MerchantKycDocument(Guid id, Guid merchantId, string type, string number, bool isVerified = false)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new BusinessRuleException("Document type cannot be empty");

        if (string.IsNullOrWhiteSpace(number))
            throw new BusinessRuleException("Document number cannot be empty");

        MerchantId = merchantId;
        DocumentType = type;
        DocumentNumber = number;
        IsVerified = isVerified;
    }

    /// <summary>
    /// Marks the document as verified.
    /// </summary>
    public void Verify()
    {
        IsVerified = true;
    }
}
