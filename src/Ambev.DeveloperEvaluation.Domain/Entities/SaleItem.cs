using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale.
/// This entity encapsulates product information, pricing, discounts,
/// and business rules related to quantity-based discounts and cancellation.
/// </summary>
public class SaleItem
{
    /// <summary>
    /// Gets the unique identifier of the sale item.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets or sets the identifier of the sale to which this item belongs.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets the external identifier of the product associated with this sale item.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the denormalized description of the product.
    /// This value is stored to avoid cross-domain joins.
    /// </summary>
    public string ProductDescription { get; private set; }

    /// <summary>
    /// Gets the quantity of the product sold.
    /// Business rules enforce minimum and maximum allowed quantities.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount applied to this sale item.
    /// The discount is calculated based on quantity-based business rules.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total monetary amount of this sale item.
    /// This value represents (Quantity × UnitPrice) minus Discount.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this sale item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets or sets the sale aggregate to which this item belongs.
    /// </summary>
    public Sale Sale { get; set; }

    /// <summary>
    /// Gets the product associated with this sale item using the External Identity pattern.
    /// </summary>
    public ExternalIdentity Product { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class.
    /// This constructor is required for ORM usage.
    /// </summary>
    protected SaleItem() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleItem"/> class with calculated values.
    /// </summary>
    /// <param name="product">The product external identity.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <param name="discount">The discount applied to this item.</param>
    protected SaleItem(ExternalIdentity product, int quantity, decimal unitPrice, decimal discount)
    {
        Id = Guid.NewGuid();
        ProductId = product.ExternalId;
        ProductDescription = product.Description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        TotalAmount = quantity * unitPrice - discount;
    }

    /// <summary>
    /// Cancels the sale item.
    /// </summary>
    /// <remarks>
    /// Once cancelled, the item total amount is reset to zero and
    /// it is excluded from the sale total calculation.
    /// </remarks>
    public void Cancel()
    {
        IsCancelled = true;
        TotalAmount = 0;
    }

    /// <summary>
    /// Calculates the discount amount based on the quantity and unit price.
    /// </summary>
    /// <param name="quantity">The quantity of items.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <returns>
    /// The calculated discount amount.
    /// </returns>
    /// <remarks>
    /// Discount rules:
    /// <list type="bullet">
    /// <item>Quantities from 4 to 9: 10% discount</item>
    /// <item>Quantities from 10 to 20: 20% discount</item>
    /// <item>Quantities below 4: no discount</item>
    /// </list>
    /// </remarks>
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

    /// <summary>
    /// Creates or updates a sale item by applying business validation rules.
    /// </summary>
    /// <param name="product">The product external identity.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <returns>
    /// A <see cref="Result{SaleItem}"/> containing the created sale item
    /// or a failure result if validation rules are violated.
    /// </returns>
    /// <remarks>
    /// Validation rules include:
    /// <list type="bullet">
    /// <item>Quantity must be greater than zero</item>
    /// <item>Quantity must not exceed the maximum allowed limit</item>
    /// <item>Discount is calculated based on quantity thresholds</item>
    /// </list>
    /// </remarks>
    public static void CreateOrUpdate(ExternalIdentity product, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        if (quantity > 20)
        {
            throw new DomainException("Quantity must be less than 20.");
        }

        decimal discount = CalculateDiscount(quantity, unitPrice);

        var item = new SaleItem(product, quantity, unitPrice, discount);
    }
}
