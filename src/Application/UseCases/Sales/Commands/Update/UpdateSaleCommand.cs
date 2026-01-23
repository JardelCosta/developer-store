using Application.Abstractions.Messaging;
using Application.UseCases.Sales.DTOs;


namespace Application.UseCases.Sales.Commands.Update;

public class UpdateSaleCommand : ICommand<Guid>
{
    public Guid SaleId { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public ExternalIdentityDto Customer { get; set; }
    public ExternalIdentityDto Branch { get; set; }
    public List<SaleItemDto> Items { get; set; } = [];
}
