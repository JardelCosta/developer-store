using FluentValidation;

namespace Application.UseCases.Sales.Commands.Update;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(c => c.SaleId)
            .NotEmpty().WithMessage("Sale ID is required.");

        RuleFor(x => x.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

        RuleFor(x => x.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(x => x.Customer)
            .NotNull().WithMessage("Customer is required.")
            .ChildRules(customer =>
            {
                customer.RuleFor(c => c.Id).NotEmpty().WithMessage("Customer ID is required.");
                customer.RuleFor(c => c.Description).NotEmpty().WithMessage("Customer name/description is required.");
            });

        RuleFor(x => x.Branch)
            .NotNull().WithMessage("Branch is required.")
            .ChildRules(branch =>
            {
                branch.RuleFor(b => b.Id).NotEmpty().WithMessage("Branch ID is required.");
                branch.RuleFor(b => b.Description).NotEmpty().WithMessage("Branch name/description is required.");
            });

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required.")
            .ForEach(item => item.ChildRules(i =>
            {
                i.RuleFor(it => it.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
                i.RuleFor(it => it.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");
            }));
    }
}
