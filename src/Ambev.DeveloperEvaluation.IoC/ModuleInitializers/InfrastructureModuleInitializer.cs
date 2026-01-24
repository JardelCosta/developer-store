using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.DomainEvents;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddTransient<DomainEventsDispatcher>();

        builder.Services.Scan(scan => scan
            .FromAssemblies(typeof(Sale).Assembly, typeof(DeleteSaleHandler).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IDomainEventHandler<>))
                .Where(type => !type.IsAbstract && !type.IsInterface))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
    }
}