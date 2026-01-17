using Application.UseCases.Sales.Commands.CreateSale;
using Application.UseCases.Sales.DTOs;
using Mediator;

namespace Api.Endpoints.Sales;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public string SaleNumber { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public ExternalIdentityDTO Customer { get; set; } = null!;
        public ExternalIdentityDTO Branch { get; set; } = null!;
        public List<CreateSaleItemDTO> Items { get; set; } = [];
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("sales", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new CreateSaleCommand(
                request.SaleNumber,
                request.SaleDate,
                request.Customer,
                request.Branch,
                request.Items),
                cancellationToken);

            return Results.Created($"/sales/{result}", result);
        })
        //.HasPermission(PermissionRoles.Admin) //Caso queira proteger o endpoint
        .WithTags(Tags.Sales);
    }
}