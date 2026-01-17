using Domain.Common;
using Domain.Exceptions;

namespace Domain.Sales;
public class Sale : Entity
{
    private readonly List<SaleItem> _items = [];

    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }

    public ExternalIdentity Customer { get; private set; }
    public ExternalIdentity Branch { get; private set; }

    public IReadOnlyCollection<SaleItem> Items => _items;

    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    protected Sale() { }

    public Sale(string saleNumber, DateTime saleDate, ExternalIdentity customer, ExternalIdentity branch)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
    }

    public void AddItem(SaleItem item)
    {
        if (IsCancelled)
        {
            throw new DomainException("Cannot add items to a cancelled sale.");
        }

        _items.Add(item);

        RecalculateTotal();
    }

    public void Cancel()
    {
        if (IsCancelled) return;

        IsCancelled = true;

        foreach (var item in _items)
        {
            item.Cancel();
        }

        TotalAmount = 0;
    }

    public void CancelItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException("Item not found.");

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
