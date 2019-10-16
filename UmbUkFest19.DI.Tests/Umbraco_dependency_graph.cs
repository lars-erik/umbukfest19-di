using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Umbraco.Core.Composing;
using Umbraco.Core.Composing.LightInject;
using Umbraco.Tests.TestHelpers;
using UmbUkFest19.DI.Tests.Interception;

namespace UmbUkFest19.DI.Tests
{
    [TestFixture]
    public class Umbraco_Dependency_Graph : InterceptorTestBase
    {
        protected override void Compose()
        {
            // Linefeed after inconclusive
            Console.WriteLine();

            base.Compose();

            // Initial dictionary will be wiped by CreateFactory, so we keep a clone for listing report
            Uniques = Uniques.ToDictionary(x => x.Key, x => x.Value);
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
                                $"{reg.Lifetime?.GetType().Name ?? "transient", -20} " +
                                $"{Uniques.ContainsKey(reg.ServiceType.FullName)}");

            Assert.Inconclusive(report);
        }

        [Test]
        public void Uniques_As_Ascii()
        {
            var report = Uniques.Aggregate("", (s, reg) => s + $"\n" +
                                                           $"{reg.Key,-20} " +
                                                           $"{reg.Value}");
            Assert.Inconclusive(report);
        }



    }
}
