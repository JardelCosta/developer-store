using Application.Abstractions.Data;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;
using Persistence.DomainEvents;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("DeveloperStoreDb"));
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<DomainEventsDispatcher>();
        services.Scan(scan => scan
                                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                                .AsImplementedInterfaces()
                                .WithScopedLifetime());

        return services;
    }
}
