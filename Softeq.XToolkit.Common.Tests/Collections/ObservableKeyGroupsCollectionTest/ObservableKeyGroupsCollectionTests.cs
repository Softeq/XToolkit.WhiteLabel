// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollection
{
    public class ObservableKeyGroupsCollectionTests
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

        [Theory] // ---
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
        [MemberData(nameof(CollectionHelper.CollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ClearGroups_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.CollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ClearGroups_NotifyResetEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        }

        [Fact] // ---
        public void CollectionChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyResetEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset, new List<string> { CollectionHelper.GroupKeyFirst }));
        }

        [Fact] // ---
        public void CollectionChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
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
        public void CollectionChanged_ReplaceGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, keys));
        }

        [Fact]
        public void CollectionChanged_ReplaceGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(keys);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyReplaceEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();
            var newKeys = items.Select(x => x.Key).Distinct().Where(x => !collection.Keys.Contains(x)).ToList();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, newKeys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_AddItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_InsertItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var newKeys = items.Select(x => x.Key).Distinct().Where(x => !collection.Keys.Contains(x)).ToList();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue(),
                     (x) => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, newKeys));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_InsertItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue(),
                     (x) => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveItems_NotifyRemoveEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            List<string> removedKeys)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(
                         items,
                         (x) => x.SelectKey(),
                         (x) => x.SelectValue());

            catcher.Unsubscribe();

            if (removedKeys.Count == 0)
            {
                return;
            }

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, removedKeys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_RemoveItems_NotifyCertainNumberOfTimes(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            List<string> removedKeys)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(
                         items,
                         (x) => x.SelectKey(),
                         (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(removedKeys.Count, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceItems_NotifyReplaceEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, items.Select(x => x.Key).Distinct().ToList()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void CollectionChanged_ReplaceItems_NotifyOneTime(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

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
        public void ItemChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemChanged_RemoveGroupsExistKeys_NotifyRemoveEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, keys));
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
        [MemberData(nameof(CollectionHelper.CollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ClearGroups_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.CollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ClearGroups_NotifyResetEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        }

        [Fact] // ---
        public void ItemsChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyResetEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset, new List<string> { CollectionHelper.GroupKeyFirst }));
        }

        [Fact] // ---
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
        public void ItemsChanged_InsertGroupsKeysOnlyAllowEmptyGroupUniqueKey_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysNotContained;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
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
        public void ItemsChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.PairNotContainedKeyWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void ItemsChanged_ReplaceGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, keys));
        }

        [Fact]
        public void ItemsChanged_ReplaceGroupsKeysOnlyAllowEmptyGroupWithKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(keys);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyReplaceEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_AddItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();
            var newKeys = items.Select(x => x.Key).Distinct().Where(x => !collection.Keys.Contains(x)).ToList();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, newKeys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_AddItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_InsertItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var newKeys = items.Select(x => x.Key).Distinct().Where(x => !collection.Keys.Contains(x)).ToList();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue(),
                (x) => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, newKeys));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_InsertItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var items = CollectionHelper.CreateFillItemsListWithExistKeys();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue(),
                (x) => x.SelectIndex());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_RemoveItems_NotifyRemoveEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            List<string> removedKeys)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue());

            catcher.Unsubscribe();

            if (removedKeys.Count == 0)
            {
                return;
            }

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, removedKeys));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_RemoveItems_NotifyCertainNumberOfTimes(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            List<string> removedKeys)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(removedKeys.Count, catcher.EventCount);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ReplaceItems_NotifyReplaceEvent(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceItems(
                items,
                (x) => x.SelectKey(),
                (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, items.Select(x => x.Key).Distinct().ToList()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsEventsTestData), MemberType = typeof(CollectionHelper))]
        public void ItemsChanged_ReplaceItems_NotifyOneTime(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void AddGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            Assert.Throws<InvalidOperationException>(() => collection.AddGroups(CollectionHelper.KeysNull));
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.KeysNull));
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupsEmptyListOfKeys_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expected = collection.Keys.Count;

            collection.AddGroups(CollectionHelper.KeysEmpty);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact] // ---
        public void AddGroups_KeysOnlyAllowEmptyGroupsNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.KeysOneFillOneNull));
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupsUniqueKeys_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysOneFillOneEmpty;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact] // ----
        public void AddGroups_KeysOnlyAllowEmptyGroupsDuplicateKeys_ArgumentException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            Assert.Throws<ArgumentException>(() => collection.AddGroups(CollectionHelper.KeysDuplicate));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairsNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.AddGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact]
        public void AddGroups_WithItemsAllowEmptyGroupsUniqueKeysWithEmptyItems_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.AddGroups(pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact] // ---
        public void AddGroups_WithItemsForbidEmptyGroupsUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            Assert.Throws<InvalidOperationException>(() => collection.AddGroups(CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.CollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentException>(() => collection.AddGroups(CollectionHelper.PairDuplicateKeyWithItems));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithItems));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithNullItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_KeysOnlyNegativeIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(-1, CollectionHelper.KeysNotContained));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_KeysOnlyOverSizedIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(collection.Keys.Count + 1, CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void InsertGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.InsertGroups(0, CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.KeysNull));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupEmptyListOfKeys_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysEmpty;
            var expected = collection.Count + keys.Count;

            collection.InsertGroups(0, keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact] // ---
        public void InsertGroups_KeysOnlyAllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.KeysOneNull));
        }

        [Fact] // ---
        public void InsertGroups_KeysOnlyAllowEmptyGroupDuplicateKey_ArgumentException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentException>(() => collection.InsertGroups(0, CollectionHelper.KeysOneFill));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupUniqueKey_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysNotContained;
            var expected = collection.Count + keys.Count;

            collection.InsertGroups(0, keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNegativeIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(-1, CollectionHelper.PairNotContainedKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsOverSizedIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(collection.Keys.Count + 1, CollectionHelper.PairNotContainedKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairsNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.InsertGroups(0, CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithItems));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithNullItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.InsertGroups(0, pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact]
        public void InsertGroups_WithItemsAllowEmptyGroupsUniqueKeysWithEmptyItems_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.InsertGroups(0, pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void InsertGroups_WithItemsForbidEmptyGroupsUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.InsertGroups(0, CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentException>(() => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsDuplicateKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithNullItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsDuplicateKeysWithEmptyItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentException>(() => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithEmptyItem));
        }

        [Fact]
        public void ReplaceGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.ReplaceGroups(CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.KeysNull));
        }

        [Fact]
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupEmptyListOfKeys_CollectionIsEmpty()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            collection.ReplaceGroups(CollectionHelper.KeysEmpty);

            Assert.Equal(0, collection.Keys.Count);
        }

        [Fact]
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.KeysOneNull));
        }

        [Fact]
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupCorrectKeys_CollectionContainsOnlyNewKeys()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;

            collection.ReplaceGroups(keys);

            foreach (var key in keys)
            {
                Assert.True(collection.Keys.Contains(key));
            }

            Assert.Equal(keys.Count, collection.Keys.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairsNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceGroups_WithItemsEmptyListOfPairs_CollectionIsEmpty(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ReplaceGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(0, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceGroupsWithItemsNullKeyTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceGroups_WithItemsNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection, IList<KeyValuePair<string, IList<int>>> pairs)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceGroups_WithItemsCorrectKeysWithItems_CollectionContainsOnlyNewKeys(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;

            collection.ReplaceGroups(pairs);

            Assert.Equal(pairs.Count, collection.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(pairs.FirstOrDefault(x => x.Key == item.Key).Value));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceGroups_WithItemsCorrectKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact]
        public void ReplaceGroups_WithItemsAllowEmptyGroupsCorrectKeysWithEmptyItems_CollectionContainsOnlyNewKeys()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var pairs = CollectionHelper.PairWithKeyWithEmptyItem;

            collection.ReplaceGroups(pairs);

            Assert.Equal(pairs.Count, collection.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(pairs.FirstOrDefault(x => x.Key == item.Key).Value));
            }
        }

        [Fact]
        public void ReplaceGroups_WithItemsForbidEmptyGroupsCorrectKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.ReplaceGroups(CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_NullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.RemoveGroups(CollectionHelper.KeysNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_ContainsKey_CollectionSizeDecreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysOneFill;
            var expectedCount = collection.Count - keys.Count;

            collection.RemoveGroups(keys);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_MissingKey_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.RemoveGroups(CollectionHelper.KeysNotContained));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_ContainsAndMissKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.RemoveGroups(CollectionHelper.KeysOneContainedOneNotContained));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ClearGroups_EmptyCollection_CollectionEmpty(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ClearGroups();

            Assert.Equal(0, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ClearGroups_FilledCollection_CollectionCleared(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ClearGroups();

            Assert.Equal(0, collection.Count);
        }

        [Fact]
        public void ClearGroup_ForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.ClearGroup(CollectionHelper.GroupKeyFirst));
        }

        [Fact]
        public void ClearGroup_AllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.ClearGroup(null));
        }

        [Fact]
        public void ClearGroup_AllowEmptyGroupExistKey_ItemsRemoved()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var key = CollectionHelper.GroupKeyFirst;
            var startCount = collection.FirstOrDefault(x => x.Key == key)?.Count() ?? 0;

            collection.ClearGroup(key);

            var currentCount = collection.FirstOrDefault(x => x.Key == key)?.Count() ?? -1;

            Assert.True((startCount > 0) && currentCount == 0);
        }

        [Fact]
        public void ClearGroup_AllowEmptyGroupMissingKey_KeyNotFoundException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<KeyNotFoundException>(() => collection.ClearGroup(CollectionHelper.GroupKeyThird));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(
                    CollectionHelper.CreateNullItemsList(),
                    (x) => x.SelectKey(),
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.AddItems(
                     CollectionHelper.CreateEmptyItemsList(),
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullItem_NullReferenceException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<NullReferenceException>(
               () => collection.AddItems(
                   CollectionHelper.CreateFillItemsListWithNull(),
                   (x) => x.SelectKey(),
                   (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_ListCorrectItems_CollectionContainsNewItems(ObservableKeyGroupsCollection<string, int> collection)
        {
            var referenceValues = collection.ToDictionary(x => x.Key, x => x.ToList());
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();

            collection.AddItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            foreach (var item in items)
            {
                if (!referenceValues.ContainsKey(item.Key))
                {
                    referenceValues.Add(item.Key, new List<int>());
                }

                referenceValues[item.Key].Add(item.Value);
            }

            Assert.Equal(referenceValues.Count(), collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(referenceValues[item.Key]));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    null,
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    (x) => x.SelectKey(),
                    null));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    (x) => null,
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateNullItemsList(),
                    (x) => x.SelectKey(),
                    (x) => x.SelectValue(),
                    (x) => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.InsertItems(
                     CollectionHelper.CreateEmptyItemsList(),
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue(),
                     (x) => x.SelectIndex());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullItem_NullReferenceException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<NullReferenceException>(
               () => collection.InsertItems(
                   CollectionHelper.CreateFillItemsListWithNull(),
                   (x) => x.SelectKey(),
                   (x) => x.SelectValue(),
                   (x) => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.InsertItemsListItemsWithExistKeysTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_ListItemsWithExistKeys_CollectionContainsNewItems(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.InsertItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue(),
                     (x) => x.SelectIndex());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_ListItemsWithNewKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.InsertItems(
                     CollectionHelper.CreateFillItemsListWithNewKeys(),
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue(),
                     (x) => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    null,
                    (x) => x.SelectValue(),
                    (x) => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    (x) => x.SelectKey(),
                    null,
                    (x) => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullIndexSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    (x) => x.SelectKey(),
                    (x) => x.SelectValue(),
                    null));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithExistKeys(),
                    (x) => null,
                    (x) => x.SelectValue(),
                    (x) => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.InsertItemsIndexOutOfBoundsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_IndexOutOfBounds_ArgumentOutOfRangeException(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.InsertItems(
                    items,
                    (x) => x.SelectKey(),
                    (x) => x.SelectValue(),
                    (x) => index));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceItems(
                    CollectionHelper.CreateNullItemsList(),
                    (x) => x.SelectKey(),
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.ReplaceItems(
                     CollectionHelper.CreateEmptyItemsList(),
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_NullItem_NullReferenceException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<NullReferenceException>(
               () => collection.ReplaceItems(
                   CollectionHelper.CreateFillItemsListWithNull(),
                   (x) => x.SelectKey(),
                   (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsListItemsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_ListItems_CollectionContainsReplacedItems(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.ReplaceItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    null,
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    (x) => x.SelectKey(),
                    null));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceItems(
                    CollectionHelper.CreateFillItemsListWithExistKeys(),
                    (x) => null,
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(
                    CollectionHelper.CreateNullItemsList(),
                    (x) => x.SelectKey(),
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.RemoveItems(
                     CollectionHelper.CreateEmptyItemsList(),
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullItem_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
               () => collection.RemoveItems(
                   CollectionHelper.CreateFillItemsListWithNull(),
                   (x) => x.SelectKey(),
                   (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsListItemsWithExistKeysTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_ListItemsWithExistKeys_ItemsSuccessRemoved(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.RemoveItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_ItemsWithNotExistKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.RemoveItems(
                     CollectionHelper.CreateFillItemsListWithNewKeys(),
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_ItemsWithNotExistValue_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.RemoveItems(
                     CollectionHelper.CreateFillItemsListWithExistKeys(),
                     (x) => x.SelectKey(),
                     (x) => int.MaxValue));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsAllItemsForExistKeyForbidEmptyTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_AllItemsForExistKeyForbidEmptyGroups_RemoveItemsRemoveEmptyGroups(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.RemoveItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsAllItemsForExistKeyAllowEmptyTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_AllItemsForExistKeyAllowEmptyGroups_RemoveItemsSaveEmptyGroups(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.RemoveItems(
                     items,
                     (x) => x.SelectKey(),
                     (x) => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(
                    CollectionHelper.CreateFillItemsListWithExistKeys(),
                    null,
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(
                    CollectionHelper.CreateFillItemsListWithExistKeys(),
                    (x) => x.SelectKey(),
                    null));
        }

        [Theory] // ---
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(
                    CollectionHelper.CreateFillItemsListWithExistKeys(),
                    (x) => null,
                    (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void GetEnumerator_AddCorrectPairs_AllElementsPassed(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItemsWithEmpty;
            collection.AddGroups(pairs);

            var isSame = true;

            var keyEnumerator = collection.GetEnumerator();
            var isNextKey = keyEnumerator.MoveNext();

            for (int i = 0; isNextKey && isSame; i++)
            {
                var element = keyEnumerator.Current;
                isNextKey = keyEnumerator.MoveNext();

                isSame = element.Key == pairs[i].Key;

                var valEnumerator = element.GetEnumerator();
                var isNextValue = valEnumerator.MoveNext();

                for (int j = 0; isNextValue && isSame; j++)
                {
                    isSame = valEnumerator.Current == pairs[i].Value[j];
                    isNextValue = valEnumerator.MoveNext();
                }
            }

            Assert.True(isSame);
        }
    }
}
