using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;


/// <summary>
/// Represents a sale aggregate in the system.
/// This entity follows domain-driven design principles and encapsulates
/// business rules related to sales lifecycle, items management, and cancellation.
/// </summary>
public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items = [];

    /// <summary>
    /// Gets the unique identifier of the sale.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the unique sale number.
    /// This value identifies the sale within the system.
    /// </summary>
    public string SaleNumber { get; private set; }

    /// <summary>
    /// Gets the external identifier of the customer associated with the sale.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the external identifier of the branch where the sale was made.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the denormalized description of the customer.
    /// This value is stored to avoid cross-domain joins.
    /// </summary>
    public string CustomerDescription { get; private set; }

    /// <summary>
    /// Gets the denormalized description of the branch.
    /// This value is stored to avoid cross-domain joins.
    /// </summary>
    public string BranchDescription { get; private set; }

    /// <summary>
    /// Gets the date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the total monetary amount of the sale.
    /// This value is calculated based on the active sale items.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the sale has been cancelled.
    /// When cancelled, no further modifications are allowed.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets the collection of items associated with the sale.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items;

    /// <summary>
    /// Gets the customer associated with the sale using the External Identity pattern.
    /// </summary>
    public virtual ExternalIdentity Customer { get; set; }

    /// <summary>
    /// Gets the branch associated with the sale using the External Identity pattern.
    /// </summary>
    public virtual ExternalIdentity Branch { get; set; }

    /// <summary>
    /// Gets the sale current status.
    /// Indicates whether the sale is cancelled, active, or suspense in the system.
    /// </summary>
    public SaleStatus Status { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class.
    /// This constructor is required for ORM usage.
    /// </summary>
    protected Sale() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sale"/> class with required data.
    /// </summary>
    /// <param name="saleNumber">The unique sale number.</param>
    /// <param name="saleDate">The date when the sale was made.</param>
    /// <param name="customer">The customer external identity.</param>
    /// <param name="branch">The branch external identity.</param>
    public Sale(string saleNumber, DateTime saleDate, ExternalIdentity customer, ExternalIdentity branch)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customer.ExternalId;
        BranchId = branch.ExternalId;
        CustomerDescription = customer.Description;
        BranchDescription = branch.Description;
    }

    /// <summary>
    /// Updates the main information of the sale.
    /// </summary>
    /// <param name="saleNumber">The new sale number.</param>
    /// <param name="saleDate">The new sale date.</param>
    /// <param name="customer">The updated customer external identity.</param>
    /// <param name="branch">The updated branch external identity.</param>
    /// <remarks>
    /// The update operation is not allowed if the sale has been cancelled.
    /// </remarks>
    public void Update(string saleNumber, DateTime saleDate, ExternalIdentity customer, ExternalIdentity branch)
    {
        if (IsCancelled)
        {
            throw new DomainException("Sale is already cancelled");
        }

        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customer.ExternalId;
        BranchId = branch.ExternalId;
        CustomerDescription = customer.Description;
        BranchDescription = branch.Description;
    }

    /// <summary>
    /// Adds a new item to the sale.
    /// </summary>
    /// <param name="item">The sale item to be added.</param>
    /// <remarks>
    /// Items cannot be added to a cancelled sale.
    /// After adding an item, the total amount is recalculated.
    /// </remarks>
    public void AddItem(SaleItem item)
    {
        if (IsCancelled)
        {
            throw new DomainException("Can´t add item to a cancelled Sale");
        }

        _items.Add(item);
        RecalculateTotal();
    }

    /// <summary>
    /// Cancels the sale and all its items.
    /// </summary>
    /// <remarks>
    /// Once cancelled, the sale becomes immutable and its total amount is reset to zero.
    /// </remarks>
    public void Cancel()
    {
        if (IsCancelled)
        {
            return;
        }

        IsCancelled = true;

        foreach (SaleItem item in _items)
        {
            item.Cancel();
        }

        TotalAmount = 0;
    }

    /// <summary>
    /// Cancels a specific item from the sale.
    /// </summary>
    /// <param name="itemId">The identifier of the item to cancel.</param>
    /// <remarks>
    /// If the item does not exist, a failure result is returned.
    /// After cancellation, the total amount is recalculated.
    /// </remarks>
    public void CancelItem(Guid itemId)
    {
        SaleItem item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new DomainException("Sale item not found");
        }

        item.Cancel();
        RecalculateTotal();
    }

    /// <summary>
    /// Recalculates the total amount of the sale based on active items.
    /// </summary>
    private void RecalculateTotal()
    {
        TotalAmount = _items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.TotalAmount);
    }
}
