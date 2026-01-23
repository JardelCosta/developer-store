using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Queries.GetAll;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("get-all", async (
            int pageNumber,
            int pageSize,
            IQueryHandler<GetAllSalesQuery,
            List<SaleResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllSalesQuery(pageNumber, pageSize);

            Result<List<SaleResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Sales);
    }
}
