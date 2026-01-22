using Application.UseCases.Sales.DTOs;
using Domain.Sales;
using SharedKernel;

namespace Application.Mappings;

public static class SaleItemMapper
{
    public static SaleItem ToDomain(this SaleItemDto dto) => new SaleItem(new ExternalIdentity(dto.Product.Id, dto.Product.Description), dto.Quantity, dto.UnitPrice);
}
