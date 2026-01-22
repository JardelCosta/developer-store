using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Queries.List;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("list", async (
            int pageNumber,
            int pageSize,
            IQueryHandler<ListSalesQuery,
            List<SaleResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new ListSalesQuery(pageNumber, pageSize);

            Result<List<SaleResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Sales);
    }
}
