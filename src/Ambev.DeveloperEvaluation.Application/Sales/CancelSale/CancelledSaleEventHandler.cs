using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class CancelledSaleEventHandler(ILogger<CancelledSaleEventHandler> logger) : IDomainEventHandler<SaleCancelledEvent>
{
    public async Task Handle(SaleCancelledEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sale with ID {SaleId} has been cancelled.", @event.SaleId);
    }
}