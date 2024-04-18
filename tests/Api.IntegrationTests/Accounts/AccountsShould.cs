using System.Net;
using System.Net.Http.Json;
using Appointer.Api.Requests;
using Appointer.Api.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Appointer.Api.IntegrationTests.Accounts;

public class AccountsShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccountsShould(WebApplicationFactory<Program> factory)
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
    }
}
