using Application.Abstractions.Messaging;

namespace Application.UseCases.Sales.Queries.GetAll;
public sealed record GetAllSalesQuery(int PageNumber, int PageSize) : IQuery<List<SaleResponse>>;
