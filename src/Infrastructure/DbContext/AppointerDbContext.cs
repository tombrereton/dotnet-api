using Appointer.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Appointer.Infrastructure.DbContext;

public class AppointerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppointerDbContext(DbContextOptions<AppointerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<UserAccount> UserAccounts { get; set; }
}