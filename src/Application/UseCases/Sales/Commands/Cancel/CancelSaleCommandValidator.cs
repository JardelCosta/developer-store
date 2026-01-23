using FluentValidation;

namespace Application.UseCases.Sales.Commands.Cancel;

internal sealed class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
{
    public CancelSaleCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
