namespace Application.UseCases.Sales.DTOs;

public record SaleItemDto(ExternalIdentityDto Product, int Quantity, decimal UnitPrice);
