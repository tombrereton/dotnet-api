using Appointer.Web.Api.Domain.Abstractions;
using Appointer.Web.Api.Domain.Accounts;
using Appointer.Web.Api.Infrastructure.Database;

namespace Appointer.Web.Api.Infrastructure.Repositories;

internal sealed class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
{
    public UserAccountRepository(AppointerDbContext dbContext) : base(dbContext)
    {
    }
}