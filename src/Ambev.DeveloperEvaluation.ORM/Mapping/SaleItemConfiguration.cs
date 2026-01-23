using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

internal sealed class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(item => item.Id);
        builder.Property(item => item.TotalAmount).IsRequired().HasDefaultValue(0);
        builder.Property(item => item.IsCancelled).IsRequired().HasDefaultValue(false);
        builder.Property(item => item.SaleId).IsRequired();
        builder.Ignore(item => item.Product);
        builder.HasQueryFilter(item => !item.IsCancelled);
    }
}
