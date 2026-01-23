using Domain.Sales;

namespace Application.UseCases.Sales.Queries.GetAll;
public class SaleResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public string CustomerDescription { get; set; }
    public string BranchDescription { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemResponse> Items { get; set; } = new();

    public static SaleResponse Map(Sale sale)
    {
        return new SaleResponse
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            CustomerDescription = sale.CustomerDescription,
            BranchDescription = sale.BranchDescription,
            SaleDate = sale.SaleDate,
            TotalAmount = sale.TotalAmount,
            Items = [.. sale.Items.Select(item => new SaleItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductDescription = item.ProductDescription,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                TotalPrice = item.TotalAmount
            })]
        };
    }
}

public class SaleItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductDescription { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}
