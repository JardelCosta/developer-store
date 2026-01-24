using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;
    private readonly DomainEventsDispatcher? _domainEventsDispatcher;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context, DomainEventsDispatcher? domainEventsDispatcher = null)
    {
        _context = context;
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await SaveChangesAndPublishEventsAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Cancel a sale in the database
    /// </summary>
    /// <param name="sale">The sale to cancel</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancelled sale</returns>
    public async Task<Sale> CancelAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        sale.Cancel();
        _context.Sales.Update(sale);
        await SaveChangesAndPublishEventsAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(x => x.Items)
            .FirstOrDefaultAsync(sale => sale.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by their sale number
    /// </summary>
    /// <param name="saleNumber">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .FirstOrDefaultAsync(sale => sale.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await SaveChangesAndPublishEventsAsync(cancellationToken);
        return true;
    }

    private async Task SaveChangesAndPublishEventsAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync();
    }

    /// <summary>
    /// Publishes domain events for all tracked entities
    /// </summary>
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        if (_domainEventsDispatcher != null && domainEvents.Count > 0)
        {
            await _domainEventsDispatcher.DispatchAsync(domainEvents);
        }
    }
}
