using Application.Abstractions.Messaging;
using Application.UseCases.Sales.DTOs;


namespace Application.UseCases.Sales.Commands.CreateSale;

public class CreateSaleCommand : ICommand<Guid>
{
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public ExternalIdentityDto Customer { get; set; }
    public ExternalIdentityDto Branch { get; set; }
    public List<SaleItemDto> Items { get; set; } = [];
}
