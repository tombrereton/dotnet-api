using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Web.Api.Domain.Accounts;
using Teeitup.Web.Api.Infrastructure.Database;
using Teeitup.Web.Api.IntegrationTests.Helpers;

namespace Teeitup.Web.Api.IntegrationTests.Infrastructure.Publishing;

[Collection("MsSqlCollection")]
public class AppointerDbContextShould : MsSqlFixture
{
    [Fact]
    public async Task PersistUserAccount()
    {
        // arrange
        var dbContext = Services.GetRequiredService<AppointerDbContext>();
        var userAccount = UserAccount.Create("John Doe");

        // act
        dbContext.UserAccounts.Add(userAccount);
        await dbContext.SaveChangesAsync();

        // assert
        var newUser = await dbContext.UserAccounts.FindAsync(userAccount.Id);
        newUser.Should().Be(userAccount);
    }
}