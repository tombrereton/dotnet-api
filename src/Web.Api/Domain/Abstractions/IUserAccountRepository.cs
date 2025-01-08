using Teeitup.Web.Api.Domain.Accounts;

namespace Teeitup.Web.Api.Domain.Abstractions;

public interface IUserAccountRepository
{
    Task<UserAccount?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(UserAccount userAccount, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}