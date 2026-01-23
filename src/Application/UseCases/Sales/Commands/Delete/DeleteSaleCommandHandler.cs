using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Commands.Delete;

internal sealed class DeleteSaleCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteSaleCommand>
{
    public async Task<Result> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        Sale? sale = await context.Sales.SingleOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (sale is null)
        {
            return Result.Failure(SaleErrors.NotFound(command.Id.ToString()));
        }

        context.Sales.Remove(sale);

        sale.Raise(new DeletedSaleDomainEvent(sale.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
