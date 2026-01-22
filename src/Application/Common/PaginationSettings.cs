using System.ComponentModel.DataAnnotations;

namespace Application.Common;
public class PaginationSettings : IValidatableObject
{
    public int MinPageNumber { get; set; }
    public int MinPageSize { get; set; }
    public int MaxPageSize { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPageNumber < 1)
        {
            yield return new ValidationResult($"MinPageNumber deve ser maior ou igual a 1.", [nameof(MinPageNumber)]);
        }

        if (MinPageSize < 1)
        {
            yield return new ValidationResult($"MinPageSize deve ser maior ou igual a 1.", [nameof(MinPageSize)]);
        }

        if (MaxPageSize < MinPageSize)
        {
            yield return new ValidationResult($"MaxPageSize deve ser maior ou igual a MinPageSize.", [nameof(MaxPageSize)]);
        }
    }
}
