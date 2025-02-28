using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Teeitup.Web.Api.ArchitectureTests
{
    public class InfrastructureTests
    {
        private readonly Assembly _assembly = typeof(Program).Assembly;

        [Fact]
        public void DomainClasses_ShouldNotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_assembly)
                .That()
                .ResideInNamespace("Teeitup.Web.Api.Infrastructure")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api.Features")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}