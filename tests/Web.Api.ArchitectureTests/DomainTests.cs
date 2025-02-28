using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Teeitup.Web.Api.ArchitectureTests
{
    public class DomainClassesShould
    {
        private readonly Assembly _assembly = typeof(Program).Assembly;

        [Fact]
        public void NotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_assembly)
                .That()
                .ResideInNamespace("Teeitup.Web.Api.Domain")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api.Infrastructure")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }

        [Fact]
        public void NotDependOnFeatures()
        {
            var result = Types.InAssembly(_assembly)
                .That()
                .ResideInNamespace("Teeitup.Web.Api.Domain")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api.Features")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}