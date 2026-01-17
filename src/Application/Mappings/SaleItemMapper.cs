using Application.UseCases.Sales.DTOs;
using Domain;
using Domain.Sales;

namespace Application.Mappings;

public static class SaleItemMapper
{
    public static SaleItem ToDomain(this CreateSaleItemDTO dto) => new SaleItem(new ExternalIdentity(dto.Product.Id, dto.Product.Description), dto.Quantity, dto.UnitPrice);
}
