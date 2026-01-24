using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeletedSaleEventHandler(ILogger<DeletedSaleEventHandler> logger) : IDomainEventHandler<SaleDeletedEvent>
{
    public async Task Handle(SaleDeletedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Sale with ID {SaleId} has been deleted.", @event.SaleId);
    }
}