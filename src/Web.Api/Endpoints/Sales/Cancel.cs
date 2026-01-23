using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Commands.Cancel;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class Cancel : IEndpoint
{
    public sealed class Request
    {
        public Guid SaleId { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("cancel", async (
            Request request,
            ICommandHandler<CancelSaleCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CancelSaleCommand
            {
                Id = request.SaleId
            };

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Sales);
    }
}
