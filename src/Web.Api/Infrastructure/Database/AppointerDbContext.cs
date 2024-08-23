using Microsoft.EntityFrameworkCore;
using Web.Api.Domain.Accounts;
// ReSharper disable ConvertToPrimaryConstructor

namespace Web.Api.Infrastructure.Database;

public class AppointerDbContext : DbContext
{
    public AppointerDbContext(){}
    public AppointerDbContext(DbContextOptions<AppointerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }
}