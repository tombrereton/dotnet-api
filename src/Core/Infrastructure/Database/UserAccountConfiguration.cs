using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teeitup.Core.Domain.Accounts;

namespace Teeitup.Core.Infrastructure.Database;

public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.FullName)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasMany(x => x.Calendars)
            .WithOne(x => x.UserAccount)
            .HasForeignKey(x => x.UserAccountId)
            .IsRequired();

    }
}