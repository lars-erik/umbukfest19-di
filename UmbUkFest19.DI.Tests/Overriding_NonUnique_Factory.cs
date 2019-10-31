using System.Collections.Generic;
using LightInject;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.Membership;
using Umbraco.Web.Cache;
using Umbraco.Web.PublishedCache;

namespace UmbUkFest19.DI.Tests
{
    [TestFixture]
    public class Overriding_NonUnique_Factory : InterceptorTestBase
    {
        protected override void Compose()
        {
            base.Compose();

            var container = (ServiceContainer)Composition.Concrete;
            container.Decorate<IPublishedSnapshotService>((factory, existing) => new DecoratingPublishedSnapshotService(existing));
        }

        [Test]
        public void Passes_Existing_To_Decorator()
        {
            var actual = Current.Factory.GetInstance<IPublishedSnapshotService>();
            Assert.IsInstanceOf<DecoratingPublishedSnapshotService>(actual);
        }
    }

    public class DecoratingPublishedSnapshotService : IPublishedSnapshotService
    {
        private readonly IPublishedSnapshotService inner;

        public DecoratingPublishedSnapshotService(IPublishedSnapshotService inner)
        {
            this.inner = inner;
        }

        public void Dispose()
        {
            inner.Dispose();
        }

        public IPublishedSnapshot CreatePublishedSnapshot(string previewToken)
        {
            return inner.CreatePublishedSnapshot(previewToken);
        }

        public bool EnsureEnvironment(out IEnumerable<string> errors)
        {
            return inner.EnsureEnvironment(out errors);
        }

        public void Rebuild()
        {
            inner.Rebuild();
        }

        public string EnterPreview(IUser user, int contentId)
        {
            return inner.EnterPreview(user, contentId);
        }

        public void RefreshPreview(string previewToken, int contentId)
        {
            inner.RefreshPreview(previewToken, contentId);
        }

        public void ExitPreview(string previewToken)
        {
            inner.ExitPreview(previewToken);
        }

        public void Notify(ContentCacheRefresher.JsonPayload[] payloads, out bool draftChanged, out bool publishedChanged)
        {
            inner.Notify(payloads, out draftChanged, out publishedChanged);
        }

        public void Notify(MediaCacheRefresher.JsonPayload[] payloads, out bool anythingChanged)
        {
            inner.Notify(payloads, out anythingChanged);
        }

        public void Notify(ContentTypeCacheRefresher.JsonPayload[] payloads)
        {
            inner.Notify(payloads);
        }

        public void Notify(DataTypeCacheRefresher.JsonPayload[] payloads)
        {
            inner.Notify(payloads);
        }

        public void Notify(DomainCacheRefresher.JsonPayload[] payloads)
        {
            inner.Notify(payloads);
        }

        public IPublishedSnapshotAccessor PublishedSnapshotAccessor => inner.PublishedSnapshotAccessor;
    }
}
