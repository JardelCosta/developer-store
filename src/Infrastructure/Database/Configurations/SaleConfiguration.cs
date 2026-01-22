using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;
internal sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.SaleNumber).IsRequired().HasMaxLength(2000);
        builder.Property(p => p.TotalAmount).IsRequired();
        builder.Property(p => p.IsCancelled).IsRequired().HasDefaultValue(false);
        builder.Property(p => p.SaleDate).IsRequired();
        builder.Property(p => p.CustomerDescription).IsRequired().HasMaxLength(200);
        builder.Property(p => p.BranchDescription).IsRequired().HasMaxLength(200);
        builder.HasQueryFilter(t => !t.IsCancelled);
        builder.Ignore(x => x.Branch).Ignore(x => x.Customer);

        builder.HasMany(s => s.Items).WithOne(i => i.Sale).HasForeignKey(i => i.SaleId).OnDelete(DeleteBehavior.Cascade);
    }
}
