using Microsoft.EntityFrameworkCore;
using Teeitup.Core.Domain.Abstractions;
using Teeitup.Core.Infrastructure.Database;

namespace Teeitup.Core.Infrastructure.Repositories;

internal abstract class Repository<T> where T : Entity
{
    protected readonly AppointerDbContext DbContext;

    protected Repository(AppointerDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}