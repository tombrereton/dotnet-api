using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.ArchitectureTests
{
    public class InfrastructureTests
    {
        private readonly Assembly _coreAssembly = typeof(IUserAccountRepository).Assembly;

        [Fact]
        public void DomainClasses_ShouldNotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace("Teeitup.Core.Infrastructure")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}