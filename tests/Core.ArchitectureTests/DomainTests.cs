using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Teeitup.Core.Domain.Abstractions;

namespace Teeitup.Core.ArchitectureTests
{
    public class DomainClassesShould
    {
        private readonly Assembly _coreAssembly = typeof(IUserAccountRepository).Assembly;
        private const string DomainNamespace = "Teeitup.Core.Domain";

        [Fact]
        public void HaveSomeClassesInNamespace()
        {
            var result = Types.InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace(DomainNamespace)
                .Should()
                .NotBePublic()
                .GetResult();

            result.FailingTypes?.Should().NotBeNull();
            result.FailingTypes?.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void NotDependOnInfrastructure()
        {
            var result = Types.InAssembly(_coreAssembly)
                .That()
                .ResideInNamespace(DomainNamespace)
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
                .ResideInNamespace(DomainNamespace)
                .Should()
                .NotHaveDependencyOn("Teeitup.Web.Api")
                .GetResult();

            result.FailingTypes?.Should().BeSameAs([]);
        }
    }
}