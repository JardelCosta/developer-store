using Application.Abstractions.Data;
using Application.Mappings;
using Domain.Sales;
using Mediator;

namespace Application.UseCases.Sales.Commands.CreateSale;

internal sealed class CreateSaleCommandHandler(IApplicationDbContext repository) : IRequestHandler<CreateSaleCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.Customer.ToDomain(),
            command.Branch.ToDomain());

        foreach (var item in command.Items)
        {
            sale.AddItem(item.ToDomain());
        }

        sale.Raise(new CreatedSaleDomainEvent(sale.Id));

        repository.Sales.Add(sale);

        await repository.SaveChangesAsync(cancellationToken);

        return sale.Id;
    }
}
