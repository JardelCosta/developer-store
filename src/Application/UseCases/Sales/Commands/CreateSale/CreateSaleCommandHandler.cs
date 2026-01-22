using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mappings;
using Application.UseCases.Sales.DTOs;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Commands.CreateSale;

internal sealed class CreateSaleCommandHandler(IApplicationDbContext repository) : ICommandHandler<CreateSaleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        Sale? entity = await repository.Sales.AsNoTracking().SingleOrDefaultAsync(u => u.SaleNumber == command.SaleNumber, cancellationToken);
        if (entity is not null)
        {
            return Result.Failure<Guid>(SaleErrors.AlreadyExists(command.SaleNumber));
        }

        Sale sale = CreateSale(command);

        sale.Raise(new CreatedSaleDomainEvent(sale.Id));

        repository.Sales.Add(sale);

        await repository.SaveChangesAsync(cancellationToken);

        return sale.Id;
    }

    private Sale CreateSale(CreateSaleCommand command)
    {
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.Customer.ToDomain(),
            command.Branch.ToDomain());

        foreach (SaleItemDto item in command.Items)
        {
            sale.AddItem(item.ToDomain());
        }

        return sale;
    }
}
