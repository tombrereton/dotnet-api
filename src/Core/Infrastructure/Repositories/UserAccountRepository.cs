using Teeitup.Core.Domain.Abstractions;
using Teeitup.Core.Domain.Accounts;
using Teeitup.Core.Infrastructure.Database;

namespace Teeitup.Core.Infrastructure.Repositories;

internal sealed class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
{
    public UserAccountRepository(AppointerDbContext dbContext) : base(dbContext)
    {
    }
}