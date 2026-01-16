using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayFlow.Domain.Aggregates;

namespace PayFlow.Infrastructure.Persistence.Configurations;

public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable("Merchants");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.MerchantType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.ShortCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.IsActive).IsRequired();
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.CreatedBy);
        builder.Property(m => m.UpdatedBy);

        builder.Property(m => m.Version)
            .IsConcurrencyToken()
            .HasDefaultValue(1); 

        builder.HasMany(m => m.KycDocuments)
            .WithOne()
            .HasForeignKey(k => k.MerchantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.Name);
        builder.HasIndex(m => m.MerchantType);
        builder.HasIndex(m => m.ShortCode);
        builder.HasIndex(m => m.IsActive);
        builder.HasIndex(m => m.CreatedAt);

        builder.Ignore(m => m.DomainEvents);
    }
}
