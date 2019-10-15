using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Umbraco.Core.Composing;
using Umbraco.Core.Composing.LightInject;
using Umbraco.Tests.TestHelpers;
using Umbraco.Tests.Testing;

[SetUpFixture]
public class Setup_UmbracoTest_Discovery
{
    [OneTimeSetUp]
    public static void Register()
    {
        TestOptionAttributeBase.ScanAssemblies.Add(typeof(Setup_UmbracoTest_Discovery).Assembly.FullName);
    }
}

namespace UmbUkFest19.DI.Tests
{
    [TestFixture]
    public class Umbraco_Dependency_Graph : BaseWebTest
    {
        [Test]
        public void As_Ascii()
        {
            var factory = (LightInjectContainer) Current.Factory;
            var serviceContainer = (LightInject.ServiceContainer)factory.Concrete;

            var registrations = serviceContainer.AvailableServices
                .OrderBy(x => x.ServiceType.FullName)
                .Skip(110)
                .Take(20);

            var report = registrations
                .Aggregate("", 
                    (s, reg) => s + $"\n" +
                                $"{reg.ServiceType.FullName,-80} " +
                                $"{reg.ImplementingType?.FullName ?? "factory",-80}" +
                                $"{reg.Lifetime?.GetType().Name ?? "transient"}");
            Assert.Inconclusive(report
            );
        }
    }
}
