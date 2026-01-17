using Application.UseCases.Sales.DTOs;
using Mediator;

namespace Application.UseCases.Sales.Commands.CreateSale;

public record CreateSaleCommand(
                                string SaleNumber,
                                DateTime SaleDate,
                                ExternalIdentityDTO Customer,
                                ExternalIdentityDTO Branch,
                                List<CreateSaleItemDTO> Items) : IRequest<Guid>;
