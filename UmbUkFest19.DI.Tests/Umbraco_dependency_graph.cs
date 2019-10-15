using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
            Assert.Inconclusive();
        }
    }
}
