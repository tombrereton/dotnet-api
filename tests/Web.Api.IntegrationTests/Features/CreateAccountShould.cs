using System.Net;
using System.Net.Http.Json;
using Appointer.Web.Api;
using Appointer.Web.Api.Features.Accounts;
using Appointer.Web.Api.Infrastructure.Database;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.IntegrationTests.Helpers;

namespace Web.Api.IntegrationTests.Features;

public class CreateAccountShould : IClassFixture<AppointerWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreateAccountShould(AppointerWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateAndPersistAccount()
    {
        // arrange
        var client = _factory.CreateClient();
        var url = "api/accounts";
        var fullName = "Full Name";
        var request = new CreateAccount.Command(FullName: fullName);

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
        newUserAccount?.FullName.Should().Be(fullName);
    }

    [Fact]
    public async Task CreateDefaultCalendar()
    {
        // arrange
        var client = _factory.CreateClient();
        var url = "api/accounts";
        var fullName = "Default Calendar" + Guid.NewGuid();
        var request = new CreateAccount.Command(FullName: fullName);

        // act
        var result = await client.PostAsJsonAsync(url, request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<CreateAccount.CreateAccountResponse>();
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointerDbContext>();
        var newUserAccount = await dbContext
            .UserAccounts
            .Include(db => db.Calendars)
            .FirstAsync(x => x.Id == response.Id);

        newUserAccount.Should().NotBeNull();
        newUserAccount?.FullName.Should().Be(fullName);

        var defaultCalendar = newUserAccount?.Calendars.FirstOrDefault();
        defaultCalendar.Should().NotBeNull();
        defaultCalendar?.Name.Should().Be("Default");
    }
}