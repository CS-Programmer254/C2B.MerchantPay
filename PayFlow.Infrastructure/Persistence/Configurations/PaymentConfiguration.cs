using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayFlow.Domain.Aggregates;

namespace PayFlow.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.CustomerId).IsRequired();
        builder.Property(p => p.MerchantId).IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.InternalReferenceNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.ExternalReference)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.CreatedBy);
        builder.Property(p => p.UpdatedBy);

        builder.Property(p => p.Version)
            .IsConcurrencyToken()
            .HasDefaultValue(1); 

        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired()
                .HasColumnName("Amount")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency)
                .IsRequired()
                .HasColumnName("Currency")
                .HasMaxLength(3);
        });

        builder.HasIndex(p => p.CustomerId);
        builder.HasIndex(p => p.MerchantId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.InternalReferenceNumber);
        builder.HasIndex(p => p.ExternalReference);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => new { p.CustomerId, p.Status });
        builder.HasIndex(p => new { p.MerchantId, p.Status });

        builder.Ignore(p => p.DomainEvents);
    }
}
