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
                .ResideInNamespace("Teeitup.Web.Api.Domain")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api.Infrastructure")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }

        [Fact]
        public void NotDependOnFeatures()
        {
            var result = Types.InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace("Teeitup.Web.Api.Domain")
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api.Features")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}