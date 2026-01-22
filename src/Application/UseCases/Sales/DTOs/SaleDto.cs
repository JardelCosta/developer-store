namespace Application.UseCases.Sales.DTOs;

public record SaleDto(string SaleNumber, DateTime SaleDate, ExternalIdentityDto Customer, ExternalIdentityDto Branch, List<SaleItemDto> Items);
