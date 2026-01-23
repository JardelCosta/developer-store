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

    public SaleItem(ExternalIdentity product, int quantity, decimal unitPrice, decimal discount)
    {
        Id = Guid.NewGuid();
        ProductId = product.ExternalId;
        ProductDescription = product.Description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        TotalAmount = quantity * unitPrice - discount;
    }

    public void Cancel()
    {
        IsCancelled = true;
        TotalAmount = 0;
    }

    private static decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        decimal total = quantity * unitPrice;

        if (quantity >= 10)
        {
            return total * 0.20m;
        }

        if (quantity >= 4)
        {
            return total * 0.10m;
        }

        return 0;
    }

    public static Result<SaleItem> Create(ExternalIdentity product, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
        {
            return Result.Failure<SaleItem>(SaleErrors.MinQuantityInvalid(quantity));
        }

        if (quantity > 20)
        {
            return Result.Failure<SaleItem>(SaleErrors.MaxQuantityInvalid(quantity));
        }

        decimal discount = CalculateDiscount(quantity, unitPrice);

        var item = new SaleItem(product, quantity, unitPrice, discount);

        return Result.Success(item);
    }
}
