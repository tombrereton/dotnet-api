using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Teeitup.Web.Api;

namespace Web.Api.ArchitectureTests
{
    public class DomainTests
    {
        private readonly Assembly _assembly = typeof(Program).Assembly;

        [Fact]
        public void DomainClasses_ShouldNotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_assembly)
                .That()
                .ResideInNamespace("Appointer.Web.Api.Domain")
                .Should()
                .NotHaveDependencyOn("Appointer.Web.Api.Infrastructure")
                .GetResult()
                .IsSuccessful;

            result.Should().BeTrue();
        }

        [Fact]
        public void DomainClasses_ShouldNotDependOnFeatures()
        {
            var result = Types.InAssembly(_assembly)
                .That()
                .ResideInNamespace("Web.Api.Domain")
                .Should()
                .NotHaveDependencyOn("Web.Api.Features")
                .GetResult()
                .IsSuccessful;

            result.Should().BeTrue();
        }
    }
}