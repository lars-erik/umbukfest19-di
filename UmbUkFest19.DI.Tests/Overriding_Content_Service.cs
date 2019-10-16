using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Umbraco.Core.Composing;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using UmbUkFest19.DI.Tests.Decorators;

namespace UmbUkFest19.DI.Tests
{
    [TestFixture]
    public class Overriding_Unique : InterceptorTestBase
    {
        protected override void Compose()
        {
            base.Compose();

            // This test req's
            Interceptor.Decorate<IContentService, ContentServiceDecorator>();
        }

        [Test]
        public void Keeps_Original_As_Nested()
        {
            var contentService = Current.Factory.GetInstance(typeof(IContentService));
            Assert.IsInstanceOf(typeof(ContentServiceDecorator), contentService);
            Assert.IsInstanceOf(typeof(ContentService), ((ContentServiceDecorator)contentService).Inner);

        }
    }
}
