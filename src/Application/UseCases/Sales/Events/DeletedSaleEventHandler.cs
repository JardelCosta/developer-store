using Application.Abstractions.Data;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.UseCases.Sales.Events;

public class DeletedSaleEventHandler(IApplicationDbContext context, ILogger<DeletedSaleEventHandler> logger) : IDomainEventHandler<DeletedSaleDomainEvent>
{
    public async Task Handle(DeletedSaleDomainEvent @event, CancellationToken cancellationToken)
    {
        bool saleExists = await context.Sales.AnyAsync(p => p.Id == @event.SaleId, cancellationToken);
        if (saleExists)
        {
            logger.LogInformation("Sale with ID {SaleId} has been created.", @event.SaleId);
        }
    }
}
