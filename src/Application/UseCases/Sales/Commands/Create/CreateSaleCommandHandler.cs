using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mappings;
using Application.UseCases.Sales.Commands.CreateSale;
using Application.UseCases.Sales.DTOs;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Commands.Create;

internal sealed class CreateSaleCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateSaleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        Sale? entity = await context.Sales.AsNoTracking().SingleOrDefaultAsync(u => u.SaleNumber == command.SaleNumber, cancellationToken);
        if (entity is not null)
        {
            return Result.Failure<Guid>(SaleErrors.AlreadyExists(command.SaleNumber));
        }

        Result<Sale> result = CreateSale(command);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        Sale sale = result.Value;

        sale.Raise(new CreatedSaleDomainEvent(sale.Id));

        context.Sales.Add(sale);

        await context.SaveChangesAsync(cancellationToken);

        return sale.Id;
    }

    private Result<Sale> CreateSale(CreateSaleCommand command)
    {
        var sale = new Sale(
            command.SaleNumber,
            command.SaleDate,
            command.Customer.ToDomain(),
            command.Branch.ToDomain());

        foreach (SaleItemDto item in command.Items)
        {
            Result<SaleItem> result = SaleItem.Create(item.Product.ToDomain(), item.Quantity, item.UnitPrice);

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
