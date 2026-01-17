using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Developer Saled API",

            });
        });

        return services;
    }
}
