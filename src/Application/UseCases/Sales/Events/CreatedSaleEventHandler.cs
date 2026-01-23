using Application.Abstractions.Data;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.UseCases.Sales.Events;

public class CreatedSaleEventHandler(IApplicationDbContext context, ILogger<CreatedSaleEventHandler> logger) : IDomainEventHandler<CreatedSaleDomainEvent>
{
    public async Task Handle(CreatedSaleDomainEvent @event, CancellationToken cancellationToken)
    {
        bool saleExists = await context.Sales.AnyAsync(p => p.Id == @event.SaleId, cancellationToken);
        if (saleExists)
        {
            logger.LogInformation("Sale with ID {SaleId} has been created.", @event.SaleId);
        }
    }
}
