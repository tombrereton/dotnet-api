using Microsoft.EntityFrameworkCore;
using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Infrastructure.Database;
using Teeitup.Web.Api.Infrastructure.Repositories;

namespace Teeitup.Web.Api.Infrastructure;

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