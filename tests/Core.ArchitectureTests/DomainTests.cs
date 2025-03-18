using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.ArchitectureTests
{
    public class DomainClassesShould
    {
        private readonly Assembly _coreAssembly = typeof(IUserAccountRepository).Assembly;

        [Fact]
        public void NotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace("Teeitup.Core.Domain")
                .Should()
                .NotHaveDependencyOn("Teeitup.Core.Infrastructure")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }

        [Fact]
        public void NotDependOnFeatures()
        {
            var result = Types.InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace("Teeitup.Core.Domain")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}