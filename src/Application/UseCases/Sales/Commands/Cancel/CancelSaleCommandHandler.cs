using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Commands.Cancel;

internal sealed class CancelSaleCommandHandler(IApplicationDbContext repository) : ICommandHandler<CancelSaleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        Sale? sale = await repository.Sales.Include(x => x.Items).SingleOrDefaultAsync(u => u.Id == command.Id, cancellationToken);
        if (sale is null)
        {
            return Result.Failure<Guid>(SaleErrors.NotFound(command.Id.ToString()));
        }

        sale.Cancel();

        sale.Raise(new CancelledSaleDomainEvent(sale.Id));

        repository.Sales.Update(sale);

        await repository.SaveChangesAsync(cancellationToken);

        return sale.Id;
    }
}
