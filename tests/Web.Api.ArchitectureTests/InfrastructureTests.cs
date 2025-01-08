using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Teeitup.Web.Api;

namespace Web.Api.ArchitectureTests
{
    public class InfrastructureTests
    {
        private readonly Assembly _assembly = typeof(Program).Assembly;

        [Fact]
        public void DomainClasses_ShouldNotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_assembly)
                .That()
                .ResideInNamespace("Web.Api.Infrastructure")
                .Should()
                .NotHaveDependencyOn("Web.Api.Features")
                .GetResult()
                .IsSuccessful;

            result.Should().BeTrue();
        }
    }
}