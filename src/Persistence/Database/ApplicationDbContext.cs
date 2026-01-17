using Application.Abstractions.Data;
using Domain.Common;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.DomainEvents;

namespace Persistence.Database;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly DomainEventsDispatcher? _domainEventsDispatcher;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DomainEventsDispatcher? domainEventsDispatcher = null)
        : base(options)
    {
        _domainEventsDispatcher = domainEventsDispatcher;
    }

    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);

        modelBuilder.Entity<Sale>().HasQueryFilter(sale => !sale.IsCancelled);

        modelBuilder.Entity<Sale>(sale =>
        {
            sale.OwnsOne(s => s.Customer);
            sale.OwnsOne(s => s.Branch);

            sale.HasMany(s => s.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SaleItem>(item =>
        {
            item.OwnsOne(i => i.Product);
        });
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await _domainEventsDispatcher!.DispatchAsync(domainEvents);
    }
}
