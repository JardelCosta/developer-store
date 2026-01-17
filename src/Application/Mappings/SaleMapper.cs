using Application.UseCases.Sales.DTOs;
using Domain;

namespace Application.Mappings;

public static class SaleMapper
{
    public static ExternalIdentity ToDomain(this ExternalIdentityDTO dto) => new(dto.Id, dto.Description);
}
