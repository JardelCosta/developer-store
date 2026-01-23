namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public record SaleItemDto(ExternalIdentityDto Product, int Quantity, decimal UnitPrice);
