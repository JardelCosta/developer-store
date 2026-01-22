using Application.Abstractions.Data;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.UseCases.Sales.Events.CreatedSale;

public class CreatedSaleEventHandler(IApplicationDbContext context, ILogger<CreatedSaleEventHandler> logger) : IDomainEventHandler<CreatedSaleDomainEvent>
{
    public async Task Handle(CreatedSaleDomainEvent notification, CancellationToken cancellationToken)
    {
        bool saleExists = await context.Sales.AnyAsync(p => p.Id == notification.SaleId, cancellationToken);
        if (saleExists)
        {
            logger.LogInformation("Sale with ID {SaleId} has been created.", notification.SaleId);
        }
    }
}
