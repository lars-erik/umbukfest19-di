using System;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Tests.Testing;
using UmbUkFest19.DI.Tests.Decorators;

namespace UmbUkFest19.DI.Tests
{
    [TestFixture]
    [UmbracoTest(
        Database = UmbracoTestOptions.Database.NewSchemaPerTest,
        PublishedRepositoryEvents = true,
        WithApplication = true
        )]
    public class Overriding_Unique : InterceptorTestBase
    {
        protected override void Compose()
        {
            base.Compose();

            Interceptor.Decorate<IContentService, ContentServiceDecorator>();
        }

        [Test]
        public void Passes_Original_To_Decorating_Type()
        {
            var contentService = Current.Factory.GetInstance<IContentService>();
            Assert.IsInstanceOf(typeof(ContentServiceDecorator), contentService);
            Assert.IsInstanceOf(typeof(ContentService), ((ContentServiceDecorator)contentService).Inner);
        }

        [Test]
        public void Intercepts_Calls()
        {
            var contentService = Current.Factory.GetInstance<IContentService>();
            contentService.Count();
            contentService.GetByLevel(0);
            var calls = ((ContentServiceDecorator) contentService).Calls();
            Assert.That(
                calls,
                Is.EquivalentTo(new[]
                {
                    "Count(null)",
                    "GetByLevel(0)"
                })
            );
        }
    }
}
