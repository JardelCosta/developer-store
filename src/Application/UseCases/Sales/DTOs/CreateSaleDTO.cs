namespace Application.UseCases.Sales.DTOs;

public record CreateSaleDTO(string SaleNumber, DateTime SaleDate, ExternalIdentityDTO Customer, ExternalIdentityDTO Branch, List<CreateSaleItemDTO> Items);