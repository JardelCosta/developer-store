using Application.Abstractions.Data;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.UseCases.Sales.Events;

public class UpdateSaleEventHandler(IApplicationDbContext context, ILogger<UpdateSaleEventHandler> logger) : IDomainEventHandler<UpdateSaleDomainEvent>
{
    public async Task Handle(UpdateSaleDomainEvent @event, CancellationToken cancellationToken)
    {
        bool saleExists = await context.Sales.AnyAsync(p => p.Id == @event.SaleId, cancellationToken);
        if (saleExists)
        {
            logger.LogInformation("Sale with ID {SaleId} has been updated.", @event.SaleId);
        }
    }
}
