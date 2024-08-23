using FluentAssertions;
using Moq;
using Web.Api.Features.Accounts;
using Web.Api.Infrastructure.Database;

namespace Web.Api.UnitTests.Features
{
    public class CreateAccountShould
    {
        [Fact]
        public async Task ValidateCommand()
        {
            // arrange
            var validator = new CreateAccount.Validator();
            var mockDbContext = new Mock<AppointerDbContext>();
            var handler = new CreateAccount.Handler(mockDbContext.Object, validator);
            var command = new CreateAccount.Command("");

            // act
            var result = await handler.Handle(command, default);

            // assert
            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Contain("Validation");
            result.Error.Message.Should().Contain("Full Name");
        }
    }
}