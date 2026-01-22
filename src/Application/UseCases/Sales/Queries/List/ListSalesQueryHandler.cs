using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Queries.List;

internal sealed class ListSalesQueryHandler(IApplicationDbContext context) : IQueryHandler<ListSalesQuery, List<SaleResponse>>
{
    private readonly int minPageNumber = 1;
    private readonly int minPageSize = 1;
    private readonly int maxPageSize = 20;

    public async Task<Result<List<SaleResponse>>> Handle(ListSalesQuery query, CancellationToken cancellationToken)
    {
        int pageNumber = query.PageNumber;
        int pageSize = query.PageSize;
        if (pageSize < minPageSize ||
            pageSize > maxPageSize)
        {
            return Result.Failure<List<SaleResponse>>(SaleErrors.PageSizeInvalid(pageSize, minPageSize, maxPageSize));
        }

        if (pageNumber < minPageNumber)
        {
            return Result.Failure<List<SaleResponse>>(SaleErrors.PageNumberInvalid(pageNumber, minPageNumber));
        }

        List<Sale> sales = await context.Sales
                .Skip((pageNumber - minPageNumber) * pageSize)
                .Take(pageSize)
                .Include(x => x.Items)
                .ToListAsync(cancellationToken);

        return sales.Select(SaleResponse.Map).ToList();
    }
}
