using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayFlow.Domain.Entities;

namespace PayFlow.Infrastructure.Persistence.Configurations;

public class MerchantKycDocumentConfiguration : IEntityTypeConfiguration<MerchantKycDocument>
{
    public void Configure(EntityTypeBuilder<MerchantKycDocument> builder)
    {
        builder.ToTable("MerchantKycDocuments");

        builder.HasKey(k => k.Id);

        builder.Property(k => k.Id)
            .ValueGeneratedNever();

        builder.Property(k => k.MerchantId)
            .IsRequired();

        builder.Property(k => k.DocumentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(k => k.DocumentNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(k => k.IsVerified)
            .IsRequired();

        builder.Property(k => k.CreatedAt)
            .IsRequired();

        builder.Property(k => k.UpdatedAt);

        builder.Property(k => k.CreatedBy);

        builder.Property(k => k.UpdatedBy);

        builder.HasIndex(k => k.MerchantId);
        builder.HasIndex(k => k.DocumentNumber);
        builder.HasIndex(k => k.IsVerified);
        builder.HasIndex(k => new { k.MerchantId, k.DocumentType });
    }
}