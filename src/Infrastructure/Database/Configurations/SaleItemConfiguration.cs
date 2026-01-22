using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;
internal sealed class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.TotalAmount).IsRequired().HasDefaultValue(0);
        builder.Property(p => p.IsCancelled).IsRequired().HasDefaultValue(false);
        builder.Property(p => p.SaleId).IsRequired();
        builder.Ignore(x => x.Product);
        builder.HasQueryFilter(t => !t.IsCancelled);
    }
}
