using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Queries.GetById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("sales/{id:guid}", async (
            Guid id,
            IQueryHandler<GetSaleByIdQuery, SaleResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new GetSaleByIdQuery(id);

            Result<SaleResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Sales);
    }
}
