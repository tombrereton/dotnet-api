using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Domain.Accounts;
using Teeitup.Web.Api.Infrastructure.Database;

namespace Teeitup.Web.Api.Infrastructure.Repositories;

internal sealed class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
{
    public UserAccountRepository(AppointerDbContext dbContext) : base(dbContext)
    {
    }
}