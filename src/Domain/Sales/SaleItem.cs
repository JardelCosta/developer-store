using SharedKernel;

namespace Domain.Sales;

public class SaleItem
{
    public Guid Id { get; private set; }
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductDescription { get; set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public Sale Sale { get; set; }
    public ExternalIdentity Product { get; set; }

    protected SaleItem() { }

    public SaleItem(
        ExternalIdentity product,
        int quantity,
        decimal unitPrice)
    {
        ValidateQuantity(quantity);
        Id = Guid.NewGuid();
        ProductId = product.ExternalId;
        ProductDescription = product.Description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = CalculateDiscount();
        TotalAmount = quantity * unitPrice - Discount;
    }

    public void Cancel()
    {
        IsCancelled = true;
        TotalAmount = 0;
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity > 20)
        {
            throw new DomainException(SaleErrors.MaxQuantityInvalid(quantity).Description);
        }

        if (quantity <= 0)
        {
            throw new DomainException(SaleErrors.MinQuantityInvalid(quantity).Description);
        }
    }

    private decimal CalculateDiscount()
    {
        decimal total = Quantity * UnitPrice;

        if (Quantity >= 10)
        {
            return total * 0.20m;
        }

        if (Quantity >= 4)
        {
            return total * 0.10m;
        }

        return 0;
    }
}
