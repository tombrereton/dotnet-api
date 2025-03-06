using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;

namespace Teeitup.Web.Api.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddCarter();
        return services;
    }

    public static IServiceCollection AddProblemDetailsForTeeitup(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                var httpContext = context.HttpContext;
                context.ProblemDetails.Instance = $"{httpContext.Request.Method}-{httpContext.Request.Path}";
                context.ProblemDetails.Extensions.TryAdd("requestId", httpContext.TraceIdentifier);
                var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        return services;
    }
}