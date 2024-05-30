using Domain.Accounts;
using FluentAssertions;
using Infrastructure.DbContext;
using Infrastructure.IntegrationTests.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IntegrationTests.DbContext;

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