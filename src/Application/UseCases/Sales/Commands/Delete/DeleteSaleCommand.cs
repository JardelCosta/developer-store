using Application.Abstractions.Messaging;

namespace Application.UseCases.Sales.Commands.Delete;

public sealed record DeleteSaleCommand(Guid Id) : ICommand;
