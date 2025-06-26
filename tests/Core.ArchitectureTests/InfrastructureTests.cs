using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.ArchitectureTests
{
    public class InfrastructureTests
    {
        private const string InfrastructureNamespace = "Teeitup.Core.Infrastructure";
        private readonly Assembly _coreAssembly = typeof(IUserAccountRepository).Assembly;

        [Fact]
        public void HaveSomeClassesInNamespace()
        {
            var result = Types
                .InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace(InfrastructureNamespace)
                .Should()
                .NotBePublic()
                .GetResult();

            result.FailingTypes?.Should().NotBeNull();
            result.FailingTypes?.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void DomainClasses_ShouldNotDependOnInfrastructure()
        {
            var result = Types
                .InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace(InfrastructureNamespace)
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}
