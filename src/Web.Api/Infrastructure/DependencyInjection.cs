using Appointer.Web.Api.Domain.Abstractions;
using Appointer.Web.Api.Infrastructure.Database;
using Appointer.Web.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Appointer.Web.Api.Infrastructure;

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