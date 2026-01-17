using Domain.Common;

namespace Domain.Sales;

public sealed record CreatedSaleDomainEvent(Guid SaleId) : IDomainEvent;
