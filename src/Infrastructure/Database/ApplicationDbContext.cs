using Application.Abstractions.Data;
using Domain.Sales;
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel;

namespace Infrastructure.Database;

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
        modelBuilder.Ignore<ExternalIdentity>();
        modelBuilder.HasDefaultSchema(Schemas.Default);
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
