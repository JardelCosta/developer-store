using SharedKernel;

namespace Domain.Sales;

public class Sale : Entity
{
    private readonly List<SaleItem> _items = [];

    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public string CustomerDescription { get; private set; }
    public string BranchDescription { get; private set; }
    public DateTime SaleDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }


    public IReadOnlyCollection<SaleItem> Items => _items;
    public virtual ExternalIdentity Customer { get; set; }
    public virtual ExternalIdentity Branch { get; set; }

    protected Sale() { }

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

    public void AddItem(SaleItem item)
    {
        if (IsCancelled)
        {
            throw new DomainException(SaleErrors.CancelledSale().Description);
        }

        _items.Add(item);

        RecalculateTotal();
    }

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

    public void CancelItem(Guid itemId)
    {
        SaleItem item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException(SaleErrors.NotFound(itemId).Description);

        item.Cancel();
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.TotalAmount);
    }
}
