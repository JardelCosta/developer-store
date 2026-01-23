using Application.Abstractions.Data;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.UseCases.Sales.Events;
public class CancelledSaleEventHandler(IApplicationDbContext context, ILogger<CancelledSaleEventHandler> logger) : IDomainEventHandler<CancelledSaleDomainEvent>
{
    public async Task Handle(CancelledSaleDomainEvent @event, CancellationToken cancellationToken)
    {
        bool saleExists = await context.Sales.AnyAsync(p => p.Id == @event.SaleId, cancellationToken);
        if (saleExists)
        {
            logger.LogInformation("Sale with ID {SaleId} has been cancelled.", @event.SaleId);
        }
    }
}
