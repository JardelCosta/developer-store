using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Commands.Delete;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("delete/{id:guid}", async (
            Guid id,
            ICommandHandler<DeleteSaleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteSaleCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Sales);
    }
}
