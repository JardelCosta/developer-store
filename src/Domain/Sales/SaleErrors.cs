using SharedKernel;

namespace Domain.Sales;

public static class SaleErrors
{
    public static Error MaxQuantityInvalid(int quantity)
    {
        return Error.Problem("Sale.MaxQuantityExceeded", $"The quantity {quantity} is inválid. Cannot sell more than 20 identical items.");
    }

    public static Error MinQuantityInvalid(int quantity)
    {
        return Error.Problem("Sale.MinQuantityInvlid", $"The quantity {quantity} is inválid. Cannot sell less than 1 item.");
    }
    public static Error CancelledSale()
    {
        return Error.Problem("Sale.CannotAddItemToCancelledSale", "Cannot add items to a cancelled sale.");
    }

    public static Error PageSizeInvalid(int pageSize, int minPageSize, int maxPageSize)
    {
        return Error.Problem("Sale.PageSizeInvalid", $"The page size = '{pageSize}' is invalid. The page size should be a value between '{minPageSize}' and '{maxPageSize}'");
    }

    public static Error PageNumberInvalid(int pageNumber, int minPageNumber)
    {
        return Error.Problem("Sale.PageNumberInvalid", $"The page number = '{pageNumber}' is invalid. The page number size should be a value greater than or equal '{minPageNumber}'");
    }

    public static Error AlreadyDisabled(Guid id)
    {
        return Error.Problem("Sale.AlreadyDisabled", $"The sale item with Id = '{id}' is already disabled.");
    }
    public static Error AlreadyExists(string saleNumber)
    {
        return Error.Problem("Sale.AlreadyExists", $"The sale item with Number = '{saleNumber}' already exists.");
    }

    public static Error NotFound(string saleId)
    {
        return Error.NotFound("Sale.NotFound", $"The sale with the Id = '{saleId}' was not found");
    }
}
