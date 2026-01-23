using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.UseCases.Sales.Queries.GetById;

internal sealed class GetSaleByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetSaleByIdQuery, SaleResponse>
{
    public async Task<Result<SaleResponse>> Handle(GetSaleByIdQuery query, CancellationToken cancellationToken)
    {
        SaleResponse? sale = await context.Sales
            .Include(sale => sale.Items)
            .Where(sale => sale.Id == query.SaleId)
            .Select(sale => SaleResponse.Map(sale))
            .SingleOrDefaultAsync(cancellationToken);

        if (sale is null)
        {
            return Result.Failure<SaleResponse>(SaleErrors.NotFound(query.SaleId));
        }

        return sale;
    }
}
