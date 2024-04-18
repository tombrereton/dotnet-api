using Appointer.Domain.Accounts;
using Appointer.Infrastructure.DbContext;
using Appointer.Infrastructure.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Appointer.Infrastructure.IntegrationTests.DbContext;

public class AppointerDbContextShould : MsSqlContainerStartup
{
    [Fact]
    public async Task PersistUserAccount()
    {
        // arrange
        var dbContext = Services.GetRequiredService<AppointerDbContext>();
        var userAccount = new UserAccount(Guid.NewGuid(), "Full Name");

        // act
        dbContext.UserAccounts.Add(userAccount);
        await dbContext.SaveChangesAsync();

        // assert
        var newUser = await dbContext.UserAccounts.FindAsync(userAccount.Id);
        newUser.Should().Be(userAccount);
    }
}