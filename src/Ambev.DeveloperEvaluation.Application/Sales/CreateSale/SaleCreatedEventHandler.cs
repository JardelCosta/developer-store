using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreatedSaleEventHandler(ILogger<CreatedSaleEventHandler> logger) : IDomainEventHandler<SaleCreatedEvent>
{
    public async Task Handle(SaleCreatedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sale with ID {SaleId} has been created.", @event.SaleId);
    }
}