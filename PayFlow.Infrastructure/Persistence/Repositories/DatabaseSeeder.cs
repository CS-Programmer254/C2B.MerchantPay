using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PayFlow.Domain.Aggregates;
using PayFlow.Domain.ValueObjects;
using PayFlow.Infrastructure.Configurations;

namespace PayFlow.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        PayFlowDbContext context,
        IOptions<DarajaOptions> darajaOptions)
    {
        if (await context.Merchants.AnyAsync())
            return;

        var shortCode = darajaOptions.Value.ShortCode.ToString();

        // ---- MERCHANTS ----
        foreach (var seed in SeedData.Merchants)
        {
            var merchant = Merchant.Create(
                seed.Name,
                seed.Type,
                shortCode 
            );

            foreach (var kyc in seed.Kyc)
            {
                merchant.AddKycDocument(
                    kyc.Type,
                    kyc.Number,
                    kyc.Verified
                );
            }

            merchant.VerifyKyc();
            context.Merchants.Add(merchant);
        }
        // ---- CUSTOMERS + WALLETS ----
        foreach (var seed in SeedData.Customers)
        {
            var customer = Customer.Create(seed.FullName, seed.PhoneNumber);
            context.Customers.Add(customer);

            var wallet = Wallet.Create(
                customer.Id,
                "Customer",
                new Money(seed.WalletBalance, "KES")
            );

            context.Wallets.Add(wallet);
        }
        await context.SaveChangesAsync();
    }
}
