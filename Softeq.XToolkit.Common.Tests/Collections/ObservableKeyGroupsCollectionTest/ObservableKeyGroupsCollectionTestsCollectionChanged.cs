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
    public class ObservableKeyGroupsCollectionTestsCollectionChanged
    {
        [Fact]
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact]
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveGroupsExistKeys_NotifyRemoveEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, keys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveGroupsExistKeys_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ClearEmptyCollection_NotNotify(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.Clear();

            catcher.Unsubscribe();

            Assert.Equal(0, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ClearFilledCollection_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.Clear();

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_Clear_NotifyResetEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.Clear();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        }

        [Fact]
        public void CollectionChanged_ClearGroupAllowEmptyExistKey_NotNotify()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.Equal(0, catcher.EventCount);
        }

        [Fact]
        public void CollectionChanged_InsertGroupsKeysOnlyAllowEmptyGroupUniqueKey_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysNotContained;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact]
        public void CollectionChanged_InsertGroupsKeysOnlyAllowEmptyGroupUniqueKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysNotContained;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, keys);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.PairNotContainedKeyWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void CollectionChanged_ReplaceAllGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyReplaceEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, keys));
        }

        [Fact]
        public void CollectionChanged_ReplaceAllGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllGroups(keys);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceAllGroupsWithItemsCorrectKeysWithItems_NotifyReplaceEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceAllGroupsWithItemsCorrectKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllGroups(pairs);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddItemsToNotExistGroups_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();
            var newKeys = items.Select(x => x.Key).Distinct().Where(x => !collection.Keys.Contains(x)).ToList();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, newKeys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddItemsToNotExistGroups_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddItemsToExistGroups_NotNotify(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(0, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_InsertItems_NotNotify(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(items, x => x.SelectKey(), x => x.SelectValue(), x => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.Equal(0, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsForbidEmptyGroupRemoveAllGroupItemsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveItemsForbidEmptyGroupRemoveAllGroupItems_NotifyRemoveEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            List<string> removedKeys)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, removedKeys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsForbidEmptyGroupRemoveAllGroupItemsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveItemsForbidEmptyGroupRemoveAllGroupItems_NotifyOneTime(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsAllowEmptyGroupRemoveAllGroupItemsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveItemsAllowEmptyGroupRemoveAllGroupItems_NotNotify(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(0, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsNotRemoveAllGroupItemsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveItemsNotRemoveAllGroupItems_NotNotify(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(0, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceAllItems_NotifyReplaceEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, items.Select(x => x.Key).Distinct().ToList()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceAllItems_NotifyOneTime(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceAllItems(items, x => x.SelectKey(), x => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }
    }
}
