using Application.UseCases.Sales.DTOs;
using SharedKernel;

namespace Application.Mappings;

public static class SaleMapper
{
    public static ExternalIdentity ToDomain(this ExternalIdentityDto dto) => new(dto.Id, dto.Description);
}
