using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Domain.Accounts;
using Web.Api.Features.Accounts;
using Web.Api.Infrastructure.Database;
using Web.Api.IntegrationTests.Helpers;

namespace Web.Api.IntegrationTests.Features;

public class AccountsShould : IClassFixture<AppointerWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccountsShould(AppointerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task BeCreatedAndPersisted()
    {
        // arrange
        var client = _factory.CreateClient();
        var url = "api/accounts";
        var fullName = "Full Name";
        var request = new CreateAccount.Command(fullName);

        // act
        var result = await client.PostAsJsonAsync(url, request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<CreateAccount.CreateAccountResponse>();
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.FullName.Should().Be(fullName);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointerDbContext>();
        var newUserAccount = await dbContext.UserAccounts.FindAsync(response.Id);
        newUserAccount.Should().NotBeNull();
        newUserAccount.FullName.Should().Be(fullName);
    }

    [Fact]
    public async Task GetExistingAccount()
    {
        // arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        var cancellationToken = cts.Token;

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointerDbContext>();
        var userAccount = new UserAccount(Guid.NewGuid(), "Existing User");
        await dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var client = _factory.CreateClient();
        var url = "api/accounts/" + userAccount.Id;

        // act
        var result = await client.GetAsync(url, cancellationToken);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<GetAccount.GetAccountResponse>(cancellationToken);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.FullName.Should().Be(userAccount.FullName);
    }
}