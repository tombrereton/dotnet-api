using Teeitup.Core.Domain.Accounts;

namespace Teeitup.Core.Domain.Abstractions;

public interface IUserAccountRepository
{
    Task<UserAccount?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(UserAccount userAccount, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}