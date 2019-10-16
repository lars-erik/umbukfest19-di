using NUnit.Framework;
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