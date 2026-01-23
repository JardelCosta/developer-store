using FluentValidation;

namespace Application.UseCases.Sales.Commands.Delete;

internal sealed class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
{
    public DeleteSaleCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
