using FluentAssertions;
using Moq;
using Teeitup.Core.Application.UserAccounts;
using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Web.Api.UnitTests.Features
{
    public class CreateAccountHandlerShould
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
            result.Value.Should().BeOfType<CreateAccount.InvalidUserAccount>();
            result.Value.As<CreateAccount.InvalidUserAccount>().Message
                .Should().Be("'Full Name' must not be empty.");
        }
    }
}