using Application.Abstractions.Messaging;

namespace Application.UseCases.Sales.Queries.GetById;

public sealed record GetSaleByIdQuery(Guid SaleId) : IQuery<SaleResponse>;
