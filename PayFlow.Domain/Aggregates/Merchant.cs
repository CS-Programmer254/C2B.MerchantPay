using PayFlow.Domain.Entities;
using PayFlow.Domain.Shared;

namespace PayFlow.Domain.Aggregates;

public class Merchant : AggregateRoot<Guid>
{
    public string Name { get; private set; } = null!;
    public string MerchantType { get; private set; } = null!;
    public string ShortCode { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private readonly List<MerchantKycDocument> _kycDocuments = new();
    public IReadOnlyCollection<MerchantKycDocument> KycDocuments => _kycDocuments;

    protected Merchant() { } // EF

    private Merchant(Guid id, string name, string type, string shortCode)
        : base(id)
    {
        Name = name;
        MerchantType = type;
        ShortCode = shortCode;
        IsActive = false;
    }

    public static Merchant Create(string name, string type, string shortCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException("Merchant name is required");

        if (string.IsNullOrWhiteSpace(type))
            throw new BusinessRuleException("Merchant type is required");

        if (string.IsNullOrWhiteSpace(shortCode))
            throw new BusinessRuleException("ShortCode is required");

        return new Merchant(
            Guid.NewGuid(),
            name,
            type,
            shortCode
        );
    }

    public void AddKycDocument(string documentType, string documentNumber, bool isVerified = false)
    {
        if (string.IsNullOrWhiteSpace(documentType))
            throw new BusinessRuleException("Document type cannot be empty");

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new BusinessRuleException("Document number cannot be empty");

        var doc = new MerchantKycDocument(
            Guid.NewGuid(),
            Id,
            documentType,
            documentNumber
        );

        if (isVerified)
            doc.Verify();

        _kycDocuments.Add(doc);
        IncrementVersion();
    }

    public void VerifyKyc()
    {
        if (!_kycDocuments.Any(d => d.IsVerified))
            throw new BusinessRuleException("KYC not verified");

        IsActive = true;
        IncrementVersion();
    }
}
