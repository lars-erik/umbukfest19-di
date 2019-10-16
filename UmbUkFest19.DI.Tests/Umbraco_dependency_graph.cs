using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private Dictionary<string, Action<IRegister>> uniques;
        private ConsoleLoggingRegistryInterceptor interceptor;

        protected override void Compose()
        {
            Console.WriteLine();

            base.Compose();

            interceptor = new ConsoleLoggingRegistryInterceptor(
                new DecoratingRegistryInterceptor(
                    Composition
                )
            );

            uniques = (Dictionary<string, Action<IRegister>>)Composition
                .GetType()
                .GetField("_uniques", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(Composition);
            var keys = uniques.Keys.ToArray();

            // Route registration through decorator
            foreach (var key in keys)
            {
                var oldFactory = uniques[key];
                uniques[key] = r => oldFactory(interceptor);
            }

            // Initial dictionary will be wiped by base, so we keep a clone for listing report
            uniques = uniques.ToDictionary(x => x.Key, x => x.Value);
        }

        [Test]
        public void Lightinject_As_Ascii()
        {
            var factory = (LightInjectContainer) Current.Factory;
            var serviceContainer = (LightInject.ServiceContainer)factory.Concrete;

            var registrations = serviceContainer.AvailableServices
                .OrderBy(x => x.ServiceType.FullName);

            var report = registrations
                .Aggregate("", 
                    (s, reg) => s + $"\n" +
                                $"{reg.ServiceType.FullName,-80} " +
                                $"{reg.ImplementingType?.FullName ?? "factory",-80}" +
                                $"{reg.Lifetime?.GetType().Name ?? "transient"} " +
                                $"{uniques.ContainsKey(reg.ServiceType.FullName)}");

            Assert.Inconclusive(report);
        }

        [Test]
        public void Uniques_As_Ascii()
        {
            var report = uniques.Aggregate("", (s, reg) =>
            {
                return s + $"\n" +
                       $"{reg.Key,-20} " +
                       $"{reg.Value}";
            });
            Assert.Inconclusive(report);
        }



    }
}
