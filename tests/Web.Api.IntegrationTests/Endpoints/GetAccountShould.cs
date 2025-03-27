using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Teeitup.Core.Application.UserAccounts;
using Teeitup.Core.Domain.Accounts;
using Teeitup.Core.Infrastructure.Database;
using Teeitup.Web.Api.IntegrationTests.Helpers;

namespace Teeitup.Web.Api.IntegrationTests.Endpoints;

public class GetAccountShould(TeeitupWebApplicationFactory<Program> factory)
    : IClassFixture<TeeitupWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task GetExistingAccount()
    {
        // arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));
        var cancellationToken = cts.Token;

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointerDbContext>();
        var userAccount = UserAccount.Create("John Doe");
        await dbContext.UserAccounts.AddAsync(userAccount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var client = _factory.CreateClient();
        var url = "api/accounts/" + userAccount.Id;

        // act
        var result = await client.GetAsync(url, cancellationToken);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await result.Content.ReadFromJsonAsync<GetAccount.Response>(cancellationToken);
        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.FullName.Should().Be(userAccount.FullName);
    }

    [Fact]
    public async Task ShowProblemDetailsIfNotFound()
    {
        // arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(30));

        var client = _factory.CreateClient();
        var notFoundId = Guid.NewGuid();
        var url = "api/accounts/" + notFoundId;

        // act
        var result = await client.GetAsync(url, cts.Token);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response = await result.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cts.Token);
        response.Should().NotBeNull();
        response!.Title.Should().Be(nameof(GetAccount.UserAccountNotFound));
        response.Detail.Should().Be("User account with id " + notFoundId + " not found.");
    }
}
