namespace PayFlow.Infrastructure.Persistence;

public static class SeedData
{
    // MERCHANTS
    public static IReadOnlyList<MerchantSeed> Merchants => new List<MerchantSeed>
    {
        new(
            Name: "Avela Ltd",
            Type: "Finance",
            Kyc: new List<KycSeed>
            {
                new("Business Permit", "BP001", true),
                new("Tax Certificate", "TCC001", true)
            }
        ),

        new(
            Name: "PayFlow Ltd",
            Type: "Payment",
            Kyc: new List<KycSeed>
            {
                new("Business Permit", "BP002", true),
                new("Tax Certificate", "TCC002", true)
            }
        ),

        new(
            Name: "Safi Stores",
            Type: "Retail",
            Kyc: new List<KycSeed>
            {
                new("Business Permit", "BP003", true),
                new("VAT Certificate", "VAT003", true)
            }
        ),

        new(
            Name: "QuickEats",
            Type: "Food",
            Kyc: new List<KycSeed>
            {
                new("Business Permit", "BP004", true),
                new("Health License", "HL004", true)
            }
        )
    };

    // CUSTOMERS
    public static IReadOnlyList<CustomerSeed> Customers => new List<CustomerSeed>
    {
        new("Uncle Bob", "254700123456", 1_000m),
        new("James Mwangi", "254701234567", 500m),
        new("Mary Wanjiru", "254702345678", 2_500m),
        new("Kevin Otieno", "254703456789", 750m),
        new("Faith Achieng", "254704567890", 3_200m),
        new("Brian Kimani", "254705678901", 150m),
        new("Sarah Njeri", "254706789012", 5_000m)
    };
}

public record MerchantSeed(
    string Name,
    string Type,
    IReadOnlyList<KycSeed> Kyc
);

public record KycSeed(
    string Type,
    string Number,
    bool Verified
);

public record CustomerSeed(
    string FullName,
    string PhoneNumber,
    decimal WalletBalance
);
