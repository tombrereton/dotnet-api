using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Core.Application.UserAccounts;
using Teeitup.Core.Contracts;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Web.Api.IntegrationTests.Helpers;

namespace Teeitup.Web.Api.IntegrationTests.Endpoints;

public class CreateAccountShould(TeeitupWebApplicationFactory<Program> factory)
    : IClassFixture<TeeitupWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

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
        var response = await result.Content.ReadFromJsonAsync<CreateAccount.Response>();
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
    public async Task ShowProblemDetailsWhenInvalid()
    {
        // arrange
        var client = _factory.CreateClient();
        var url = "api/accounts";
        var fullName = string.Empty;
        var request = new CreateAccount.Command(FullName: fullName);

        // act
        var result = await client.PostAsJsonAsync(url, request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await result.Content.ReadFromJsonAsync<ProblemDetails>();
        response.Should().NotBeNull();
        response!.Title.Should().Be(nameof(CreateAccount.InvalidUserAccount));
        response.Detail.Should().Be("'Full Name' must not be empty.");
    }

    [Fact]
    public async Task PublishUserAccountCreatedIntegrationEvent()
    {
        // arrange
        var testHarness = _factory.Services.GetTestHarness();
        await testHarness.Start();
        var client = _factory.CreateClient();
        var url = "api/accounts";
        var fullName = "Default Calendar" + Guid.NewGuid();
        var request = new CreateAccount.Command(FullName: fullName);

        // act
        var result = await client.PostAsJsonAsync(url, request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<CreateAccount.Response>();
        var integrationEvent = testHarness.Published
        .Select<UserAccountCreatedIntegrationEvent>()
        .First(x => x.Context.Message.UserAccountId == response?.Id)
        .Context.Message;
        integrationEvent.FullName.Should().Be(fullName);
    }
}
