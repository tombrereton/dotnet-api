using System.Net;
using System.Net.Http.Json;
using Appointer.Api.IntegrationTests.Helpers;
using Appointer.Api.Requests;
using Appointer.Api.Responses;
using Appointer.Infrastructure.DbContext;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Appointer.Api.IntegrationTests.Accounts;

public class AccountsShould : IClassFixture<AppointerWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccountsShould(AppointerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAccount()
    {
        // arrange
        var client = _factory.CreateClient();
        var url = "api/account";
        var fullName = "Full Name";
        var request = new CreateAccountRequest(fullName);

        // act
        var result = await client.PostAsJsonAsync(url, request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<CreateAccountResponse>();
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.FullName.Should().Be(fullName);


        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointerDbContext>();
        var newItem = await dbContext.UserAccounts.FindAsync(response.Id);
        newItem.Should().NotBeNull();
        newItem.FullName.Should().Be(fullName);
    }
}