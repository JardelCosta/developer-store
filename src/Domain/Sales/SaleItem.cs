using Domain.Exceptions;

namespace Domain.Sales;

public class SaleItem
{
    public Guid Id { get; private set; }
    public ExternalIdentity Product { get; private set; }

    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    protected SaleItem() { }

    public SaleItem(
        ExternalIdentity product,
        int quantity,
        decimal unitPrice)
    {
        ValidateQuantity(quantity);

        Id = Guid.NewGuid();
        Product = product;
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
            throw new DomainException("Cannot sell more than 20 identical items.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");
    }

    private decimal CalculateDiscount()
    {
        var total = Quantity * UnitPrice;

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
