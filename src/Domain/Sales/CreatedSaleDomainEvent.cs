using SharedKernel;

namespace Domain.Sales;

public sealed record CreatedSaleDomainEvent(Guid SaleId) : IDomainEvent;
