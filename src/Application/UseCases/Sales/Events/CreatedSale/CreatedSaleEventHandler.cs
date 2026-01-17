using Application.Abstractions.Data;
using Domain.Common;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Sales.Events.CreatedSale;

public class CreatedSaleEventHandler(IApplicationDbContext context) : IDomainEventHandler<CreatedSaleDomainEvent>
{
    public async Task Handle(CreatedSaleDomainEvent notification, CancellationToken cancellationToken)
    {
        bool saleExists = await context.Sales.AnyAsync(p => p.Id == notification.SaleId, cancellationToken);
        if (saleExists)
        {
            Console.WriteLine($"Sale with ID {notification.SaleId} has been created.");
        }
    }
}
