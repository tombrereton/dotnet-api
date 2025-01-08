using Appointer.Web.Api.Domain.Calendars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointer.Web.Api.Infrastructure.Database;

public class CalendarConfiguration : IEntityTypeConfiguration<Calendar>
{
    public void Configure(EntityTypeBuilder<Calendar> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
            .Property(x => x.Name)
            .IsRequired();

        builder
            .HasOne(x => x.UserAccount)
            .WithMany(x => x.Calendars)
            .HasForeignKey(x => x.UserAccountId)
            .IsRequired();

    }
}