using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Api.Domain.Abstractions;
using Web.Api.Infrastructure.Database;
using Web.Api.Infrastructure.Repositories;

namespace Web.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppointerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database")));

        services.AddTransient<IUserAccountRepository, UserAccountRepository>();

        return services;
    }
}