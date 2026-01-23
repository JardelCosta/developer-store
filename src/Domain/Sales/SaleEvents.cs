using SharedKernel;

namespace Domain.Sales;

public sealed record CreatedSaleDomainEvent(Guid SaleId) : IDomainEvent;
public sealed record CancelledSaleDomainEvent(Guid SaleId) : IDomainEvent;
public sealed record DeletedSaleDomainEvent(Guid SaleId) : IDomainEvent;
