// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTestsItemsChanged
    {
        [Fact]
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemChanged_RemoveGroupsExistKeys_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_Clear_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.Clear();

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyResetEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        }

        [Fact]
        public void ItemsChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_InsertGroupsKeysOnlyAllowEmptyGroupUniqueKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.KeysNotContained);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_InsertGroupsWithItemsUniqueKeysWithItemsNotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.PairNotContainedKeyWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_ReplaceAllGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllGroups(keys);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ReplaceAllGroupsWithItemsCorrectKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllGroups(pairs);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_AddItemsWithExistKeys_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var newKeys = items.Select(x => x.Key).Distinct().Where(x => !collection.Keys.Contains(x)).ToList();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_AddItemsWithExistKeys_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, items.Select(x => x.Value).ToList()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_AddItemsWithNewKeys_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithOnlyNewKeys();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_InsertItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(items, (x) => x.SelectKey(), (x) => x.SelectValue(), (x) => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, items.Select(x => x.Value).ToList()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_InsertItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(items, (x) => x.SelectKey(), (x) => x.SelectValue(), (x) => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_RemoveItemsForbidEmptyGroupRemoveAllGroupItems_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            var items = CollectionHelper.RemoveItemsOnlyExistItemsForFirstGroup();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_RemoveItemsAllowEmptyGroupRemoveAllGroupItems_NotifyRemoveEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var items = CollectionHelper.RemoveItemsOnlyExistItemsForFirstGroup();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, items.Select(x => x.Value).ToList()));
        }

        [Fact]
        public void ItemsChanged_RemoveItemsAllowEmptyGroupRemoveAllGroupItems_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var items = CollectionHelper.RemoveItemsOnlyExistItemsForFirstGroup();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsNotRemoveAllGroupItemsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_RemoveItemsNotRemoveAllGroupItems_NotifyRemoveEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, items.Select(x => x.Value).ToList()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsNotRemoveAllGroupItemsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_RemoveItemsNotRemoveAllGroupItems_NotifyOneTime(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ReplaceAllItems_NotifyOneTime(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }
    }
}
