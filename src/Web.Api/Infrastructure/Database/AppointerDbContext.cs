using Microsoft.EntityFrameworkCore;
using Web.Api.Domain.Accounts;
// ReSharper disable ConvertToPrimaryConstructor

namespace Web.Api.Infrastructure.Database;

public class AppointerDbContext : DbContext
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