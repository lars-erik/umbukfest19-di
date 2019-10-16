using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Umbraco.Core.Composing;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Tests.TestHelpers;

namespace UmbUkFest19.DI.Tests
{
    [TestFixture]
    public class Overriding_Unique : BaseWebTest
    {
        private Dictionary<string, Action<IRegister>> uniques;
        private IRegister interceptor;

        protected override void Compose()
        {
            Console.WriteLine();

            base.Compose();

            var decoratingInterceptor = new DecoratingRegistryInterceptor(
                Composition
            );
            interceptor = //new ConsoleLoggingRegistryInterceptor(
                decoratingInterceptor
            //)
            ;

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

            // This test req's
            decoratingInterceptor.Decorate<IContentService, ContentServiceDecorator>();
        }

        [Test]
        public void Keeps_Original_As_Nested()
        {
            var contentService = Current.Factory.GetInstance(typeof(IContentService));
            Assert.IsInstanceOf(typeof(ContentServiceDecorator), contentService);
            Assert.IsInstanceOf(typeof(ContentService), ((ContentServiceDecorator)contentService).Inner);

        }
    }

    public class ContentServiceDecorator : IContentService
    {
        private IContentService inner;

        public ContentServiceDecorator(ContentService inner)
        {
            this.inner = inner;
        }

        public IContentService Inner => inner;

        public IContent GetBlueprintById(int id)
        {
            return inner.GetBlueprintById(id);
        }

        public IContent GetBlueprintById(Guid id)
        {
            return inner.GetBlueprintById(id);
        }

        public IEnumerable<IContent> GetBlueprintsForContentTypes(params int[] documentTypeId)
        {
            return inner.GetBlueprintsForContentTypes(documentTypeId);
        }

        public void SaveBlueprint(IContent content, int userId = -1)
        {
            inner.SaveBlueprint(content, userId);
        }

        public void DeleteBlueprint(IContent content, int userId = -1)
        {
            inner.DeleteBlueprint(content, userId);
        }

        public IContent CreateContentFromBlueprint(IContent blueprint, string name, int userId = -1)
        {
            return inner.CreateContentFromBlueprint(blueprint, name, userId);
        }

        public void DeleteBlueprintsOfType(int contentTypeId, int userId = -1)
        {
            inner.DeleteBlueprintsOfType(contentTypeId, userId);
        }

        public void DeleteBlueprintsOfTypes(IEnumerable<int> contentTypeIds, int userId = -1)
        {
            inner.DeleteBlueprintsOfTypes(contentTypeIds, userId);
        }

        public IContent GetById(int id)
        {
            return inner.GetById(id);
        }

        public IContent GetById(Guid key)
        {
            return inner.GetById(key);
        }

        public IEnumerable<IContent> GetByIds(IEnumerable<int> ids)
        {
            return inner.GetByIds(ids);
        }

        public IEnumerable<IContent> GetByIds(IEnumerable<Guid> ids)
        {
            return inner.GetByIds(ids);
        }

        public IEnumerable<IContent> GetByLevel(int level)
        {
            return inner.GetByLevel(level);
        }

        public IContent GetParent(int id)
        {
            return inner.GetParent(id);
        }

        public IContent GetParent(IContent content)
        {
            return inner.GetParent(content);
        }

        public IEnumerable<IContent> GetAncestors(int id)
        {
            return inner.GetAncestors(id);
        }

        public IEnumerable<IContent> GetAncestors(IContent content)
        {
            return inner.GetAncestors(content);
        }

        public IEnumerable<IContent> GetVersions(int id)
        {
            return inner.GetVersions(id);
        }

        public IEnumerable<IContent> GetVersionsSlim(int id, int skip, int take)
        {
            return inner.GetVersionsSlim(id, skip, take);
        }

        public IEnumerable<int> GetVersionIds(int id, int topRows)
        {
            return inner.GetVersionIds(id, topRows);
        }

        public IContent GetVersion(int versionId)
        {
            return inner.GetVersion(versionId);
        }

        public IEnumerable<IContent> GetRootContent()
        {
            return inner.GetRootContent();
        }

        public IEnumerable<IContent> GetContentForExpiration(DateTime date)
        {
            return inner.GetContentForExpiration(date);
        }

        public IEnumerable<IContent> GetContentForRelease(DateTime date)
        {
            return inner.GetContentForRelease(date);
        }

        public IEnumerable<IContent> GetPagedContentInRecycleBin(long pageIndex, int pageSize, out long totalRecords, IQuery<IContent> filter = null,
            Ordering ordering = null)
        {
            return inner.GetPagedContentInRecycleBin(pageIndex, pageSize, out totalRecords, filter, ordering);
        }

        public IEnumerable<IContent> GetPagedChildren(int id, long pageIndex, int pageSize, out long totalRecords, IQuery<IContent> filter = null,
            Ordering ordering = null)
        {
            return inner.GetPagedChildren(id, pageIndex, pageSize, out totalRecords, filter, ordering);
        }

        public IEnumerable<IContent> GetPagedDescendants(int id, long pageIndex, int pageSize, out long totalRecords, IQuery<IContent> filter = null,
            Ordering ordering = null)
        {
            return inner.GetPagedDescendants(id, pageIndex, pageSize, out totalRecords, filter, ordering);
        }

        public IEnumerable<IContent> GetPagedOfType(int contentTypeId, long pageIndex, int pageSize, out long totalRecords, IQuery<IContent> filter,
            Ordering ordering = null)
        {
            return inner.GetPagedOfType(contentTypeId, pageIndex, pageSize, out totalRecords, filter, ordering);
        }

        public IEnumerable<IContent> GetPagedOfTypes(int[] contentTypeIds, long pageIndex, int pageSize, out long totalRecords, IQuery<IContent> filter,
            Ordering ordering = null)
        {
            return inner.GetPagedOfTypes(contentTypeIds, pageIndex, pageSize, out totalRecords, filter, ordering);
        }

        public int Count(string documentTypeAlias = null)
        {
            return inner.Count(documentTypeAlias);
        }

        public int CountPublished(string documentTypeAlias = null)
        {
            return inner.CountPublished(documentTypeAlias);
        }

        public int CountChildren(int parentId, string documentTypeAlias = null)
        {
            return inner.CountChildren(parentId, documentTypeAlias);
        }

        public int CountDescendants(int parentId, string documentTypeAlias = null)
        {
            return inner.CountDescendants(parentId, documentTypeAlias);
        }

        public bool HasChildren(int id)
        {
            return inner.HasChildren(id);
        }

        public OperationResult Save(IContent content, int userId = -1, bool raiseEvents = true)
        {
            return inner.Save(content, userId, raiseEvents);
        }

        public OperationResult Save(IEnumerable<IContent> contents, int userId = -1, bool raiseEvents = true)
        {
            return inner.Save(contents, userId, raiseEvents);
        }

        public OperationResult Delete(IContent content, int userId = -1)
        {
            return inner.Delete(content, userId);
        }

        public void DeleteOfType(int documentTypeId, int userId = -1)
        {
            inner.DeleteOfType(documentTypeId, userId);
        }

        public void DeleteOfTypes(IEnumerable<int> contentTypeIds, int userId = -1)
        {
            inner.DeleteOfTypes(contentTypeIds, userId);
        }

        public void DeleteVersions(int id, DateTime date, int userId = -1)
        {
            inner.DeleteVersions(id, date, userId);
        }

        public void DeleteVersion(int id, int versionId, bool deletePriorVersions, int userId = -1)
        {
            inner.DeleteVersion(id, versionId, deletePriorVersions, userId);
        }

        public void Move(IContent content, int parentId, int userId = -1)
        {
            inner.Move(content, parentId, userId);
        }

        public IContent Copy(IContent content, int parentId, bool relateToOriginal, int userId = -1)
        {
            return inner.Copy(content, parentId, relateToOriginal, userId);
        }

        public IContent Copy(IContent content, int parentId, bool relateToOriginal, bool recursive, int userId = -1)
        {
            return inner.Copy(content, parentId, relateToOriginal, recursive, userId);
        }

        public OperationResult MoveToRecycleBin(IContent content, int userId = -1)
        {
            return inner.MoveToRecycleBin(content, userId);
        }

        public OperationResult EmptyRecycleBin()
        {
            return inner.EmptyRecycleBin();
        }

        public OperationResult EmptyRecycleBin(int userId = -1)
        {
            return inner.EmptyRecycleBin(userId);
        }

        public OperationResult Sort(IEnumerable<IContent> items, int userId = -1, bool raiseEvents = true)
        {
            return inner.Sort(items, userId, raiseEvents);
        }

        public OperationResult Sort(IEnumerable<int> ids, int userId = -1, bool raiseEvents = true)
        {
            return inner.Sort(ids, userId, raiseEvents);
        }

        public PublishResult SaveAndPublish(IContent content, string culture = "*", int userId = -1, bool raiseEvents = true)
        {
            return inner.SaveAndPublish(content, culture, userId, raiseEvents);
        }

        public PublishResult SaveAndPublish(IContent content, string[] cultures, int userId = -1, bool raiseEvents = true)
        {
            return inner.SaveAndPublish(content, cultures, userId, raiseEvents);
        }

        public IEnumerable<PublishResult> SaveAndPublishBranch(IContent content, bool force, string culture = "*", int userId = -1)
        {
            return inner.SaveAndPublishBranch(content, force, culture, userId);
        }

        public IEnumerable<PublishResult> SaveAndPublishBranch(IContent content, bool force, string[] cultures, int userId = -1)
        {
            return inner.SaveAndPublishBranch(content, force, cultures, userId);
        }

        public PublishResult Unpublish(IContent content, string culture = "*", int userId = -1)
        {
            return inner.Unpublish(content, culture, userId);
        }

        public bool IsPathPublishable(IContent content)
        {
            return inner.IsPathPublishable(content);
        }

        public bool IsPathPublished(IContent content)
        {
            return inner.IsPathPublished(content);
        }

        public bool SendToPublication(IContent content, int userId = -1)
        {
            return inner.SendToPublication(content, userId);
        }

        public IEnumerable<PublishResult> PerformScheduledPublish(DateTime date)
        {
            return inner.PerformScheduledPublish(date);
        }

        public EntityPermissionCollection GetPermissions(IContent content)
        {
            return inner.GetPermissions(content);
        }

        public void SetPermissions(EntityPermissionSet permissionSet)
        {
            inner.SetPermissions(permissionSet);
        }

        public void SetPermission(IContent entity, char permission, IEnumerable<int> groupIds)
        {
            inner.SetPermission(entity, permission, groupIds);
        }

        public IContent Create(string name, Guid parentId, string documentTypeAlias, int userId = -1)
        {
            return inner.Create(name, parentId, documentTypeAlias, userId);
        }

        public IContent Create(string name, int parentId, string documentTypeAlias, int userId = -1)
        {
            return inner.Create(name, parentId, documentTypeAlias, userId);
        }

        public IContent Create(string name, IContent parent, string documentTypeAlias, int userId = -1)
        {
            return inner.Create(name, parent, documentTypeAlias, userId);
        }

        public IContent CreateAndSave(string name, int parentId, string contentTypeAlias, int userId = -1)
        {
            return inner.CreateAndSave(name, parentId, contentTypeAlias, userId);
        }

        public IContent CreateAndSave(string name, IContent parent, string contentTypeAlias, int userId = -1)
        {
            return inner.CreateAndSave(name, parent, contentTypeAlias, userId);
        }

        public OperationResult Rollback(int id, int versionId, string culture = "*", int userId = -1)
        {
            return inner.Rollback(id, versionId, culture, userId);
        }
    }
}
