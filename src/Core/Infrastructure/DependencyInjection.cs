using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Core.Domain.Abstractions;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Core.Infrastructure.Repositories;

namespace Teeitup.Core.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppointerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("database")));

        services.AddTransient<IUserAccountRepository, UserAccountRepository>();

        return services;
    }
}