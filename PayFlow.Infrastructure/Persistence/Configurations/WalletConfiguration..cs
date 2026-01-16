using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayFlow.Domain.Aggregates;

namespace PayFlow.Infrastructure.Persistence.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).ValueGeneratedNever();
        builder.Property(w => w.OwnerId).IsRequired();
        builder.Property(w => w.OwnerType).IsRequired().HasMaxLength(50);
        builder.Property(w => w.CreatedAt).IsRequired();
        builder.Property(w => w.UpdatedAt);
        builder.Property(w => w.CreatedBy);
        builder.Property(w => w.UpdatedBy);

        builder.Property(w => w.Version)
            .IsConcurrencyToken()
            .HasDefaultValue(1); 

        builder.OwnsOne(w => w.Balance, balance =>
        {
            balance.OwnsOne(b => b.Value);
        });

        builder.HasIndex(w => w.OwnerId).IsUnique();
        builder.HasIndex(w => w.OwnerType);
        builder.HasIndex(w => new { w.OwnerId, w.OwnerType });
        builder.HasIndex(w => w.CreatedAt);

        builder.Ignore(w => w.DomainEvents);
    }
}
