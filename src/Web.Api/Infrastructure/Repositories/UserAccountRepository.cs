using Web.Api.Domain.Abstractions;
using Web.Api.Domain.Accounts;
using Web.Api.Infrastructure.Database;

namespace Web.Api.Infrastructure.Repositories;

internal sealed class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
{
    public UserAccountRepository(AppointerDbContext dbContext) : base(dbContext)
    {
    }
}