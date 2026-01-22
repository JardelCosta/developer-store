using Application.Abstractions.Messaging;

namespace Application.UseCases.Sales.Queries.List;
public sealed record ListSalesQuery(int PageNumber, int PageSize) : IQuery<List<SaleResponse>>;
