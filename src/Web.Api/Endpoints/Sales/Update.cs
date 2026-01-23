using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Commands.Update;
using Application.UseCases.Sales.DTOs;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class Update : IEndpoint
{
    public sealed class Request
    {
        public Guid SaleId { get; set; }
        public string SaleNumber { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public ExternalIdentityDto Customer { get; set; } = null!;
        public ExternalIdentityDto Branch { get; set; } = null!;
        public List<SaleItemDto> Items { get; set; } = [];
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("update", async (
            Request request,
            ICommandHandler<UpdateSaleCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateSaleCommand
            {
                SaleId = request.SaleId,
                SaleNumber = request.SaleNumber,
                SaleDate = request.SaleDate,
                Customer = request.Customer,
                Branch = request.Branch,
                Items = request.Items
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(
                value => Results.Created($"/sales/{value}", value),
                error => CustomResults.Problem(error)
            );

        })
        .WithTags(Tags.Sales);
    }
}
