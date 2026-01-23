using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mappings;
using Application.UseCases.Sales.DTOs;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Commands.Update;

internal sealed class UpdateSaleCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateSaleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        Sale? entity = await context.Sales.SingleOrDefaultAsync(u => u.SaleNumber == command.SaleNumber, cancellationToken);
        if (entity is null)
        {
            return Result.Failure<Guid>(SaleErrors.NotFound(command.SaleNumber));
        }

        Result<Sale> result = UpdateSale(entity, command);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        Sale sale = result.Value;

        sale.Raise(new UpdateSaleDomainEvent(sale.Id));

        context.Sales.Add(sale);

        await context.SaveChangesAsync(cancellationToken);

        return sale.Id;
    }

    private Result<Sale> UpdateSale(Sale sale, UpdateSaleCommand command)
    {
        sale.Update(
            command.SaleNumber,
            command.SaleDate,
            command.Customer.ToDomain(),
            command.Branch.ToDomain());

        foreach (SaleItemDto item in command.Items)
        {
            Result<SaleItem> result = SaleItem.CreateOrUpdate(item.Product.ToDomain(), item.Quantity, item.UnitPrice);

            if (result.IsFailure)
            {
                return Result.Failure<Sale>(result.Error);
            }

            Result addResult = sale.AddItem(result.Value);

            if (addResult.IsFailure)
            {
                return Result.Failure<Sale>(addResult.Error);
            }
        }

        return Result.Success(sale);
    }

}
