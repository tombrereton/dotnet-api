using FluentAssertions;
using Moq;
using Teeitup.Web.Api.Domain.Abstractions;
using Teeitup.Web.Api.Features.Accounts;

namespace Web.Api.UnitTests.Features
{
    public class CreateAccountShould
    {
        [Fact]
        public async Task ValidateCommand()
        {
            // arrange
            var validator = new CreateAccount.Validator();
            var mockDbContext = new Mock<IUserAccountRepository>();
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