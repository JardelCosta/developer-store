using Application.Abstractions.Messaging;

namespace Application.UseCases.Sales.Commands.Cancel;

public class CancelSaleCommand : ICommand<Guid>
{
    public Guid Id { get; set; }
}
