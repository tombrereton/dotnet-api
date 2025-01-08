using Carter;
using FluentValidation;

namespace Teeitup.Web.Api.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
        services.AddCarter();
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}