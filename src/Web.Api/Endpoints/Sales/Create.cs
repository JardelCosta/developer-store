using Application.Abstractions.Messaging;
using Application.UseCases.Sales.Commands.CreateSale;
using Application.UseCases.Sales.DTOs;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Sales;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string SaleNumber { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public ExternalIdentityDto Customer { get; set; } = null!;
        public ExternalIdentityDto Branch { get; set; } = null!;
        public List<SaleItemDto> Items { get; set; } = [];
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("sales", async (
            Request request,
            ICommandHandler<CreateSaleCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateSaleCommand
            {
                SaleNumber = request.SaleNumber,
                SaleDate = request.SaleDate,
                Customer = request.Customer,
                Branch = request.Branch,
                Items = request.Items
            };

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Sales);
    }
}
