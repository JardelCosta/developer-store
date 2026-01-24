namespace Ambev.DeveloperEvaluation.Domain.Events;

public sealed record SaleDeletedEvent(Guid SaleId) : IDomainEvent;