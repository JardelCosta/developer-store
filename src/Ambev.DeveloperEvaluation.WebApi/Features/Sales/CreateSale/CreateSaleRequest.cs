using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public sealed class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the unique sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer associated with the sale.
    /// Uses the External Identity pattern to reference an external domain.
    /// </summary>
    public ExternalIdentityDto Customer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the branch where the sale was made.
    /// Uses the External Identity pattern to reference an external domain.
    /// </summary>
    public ExternalIdentityDto Branch { get; set; } = null!;

    /// <summary>
    /// Gets or sets the list of items included in the sale.
    /// </summary>
    public List<SaleItemDto> Items { get; set; } = [];
}
