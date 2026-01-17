namespace Application.UseCases.Sales.DTOs;

public record CreateSaleItemDTO(ExternalIdentityDTO Product, int Quantity, decimal UnitPrice);
