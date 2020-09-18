// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace ObservableKeyGroupsCollection
{
    public class ObservableKeyGroupsCollectionTests
    {
        private const string KeysParameterName = "keys";
        private const string ItemsParameterName = "items";
        private const string AddDuplicateItemExceptionMessage = "An item with the same key has already been added.";

        public static IEnumerable<object[]> CollectionOptions =>
            new List<object[]>
            {
                new object[] {CollectionHelper.CreateWithEmptyGroups() },
                new object[] {CollectionHelper.CreateWithoutEmptyGroups() },
                new object[] {CollectionHelper.CreateFilledGroupsWithEmpty() },
                new object[] {CollectionHelper.CreateFilledGroupsWithoutEmpty() }
            };

        public static IEnumerable<object[]> EmptyCollectionOptions =>
            new List<object[]>
            {
                new object[] {CollectionHelper.CreateWithEmptyGroups() },
                new object[] {CollectionHelper.CreateWithoutEmptyGroups() }
            };

        public static IEnumerable<object[]> FillCollectionOptions =>
         new List<object[]>
         {
                new object[] {CollectionHelper.CreateFilledGroupsWithEmpty() },
                new object[] {CollectionHelper.CreateFilledGroupsWithoutEmpty() }
         };

        [Fact] // +++
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

        [Fact] // +++
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void CollectionChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void CollectionChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void CollectionChanged_RemoveGroupsExistKeys_NotifyRemoveEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, keys));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void CollectionChanged_RemoveGroupsExistKeys_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void CollectionChanged_ClearGroups_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void CollectionChanged_ClearGroups_NotifyResetEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        }

        [Fact] // +++ no event type in collection code
        public void CollectionChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyResetEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset, new List<string> { CollectionHelper.GroupKeyFirst }));
        }

        [Fact] // +++
        public void CollectionChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact] // +++
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

        [Fact] // +++
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

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void CollectionChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void CollectionChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.PairNotContainedKeyWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact] // review night
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

        [Fact] // +++
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

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void CollectionChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyReplaceEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void CollectionChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact] // +++
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

        [Fact] // +++
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void ItemChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void ItemChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ItemChanged_RemoveGroupsExistKeys_NotifyRemoveEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(keys);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Remove, keys));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ItemChanged_RemoveGroupsExistKeys_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.RemoveGroups(CollectionHelper.KeysTwoFill);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void ItemsChanged_ClearGroups_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void ItemsChanged_ClearGroups_NotifyResetEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroups();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        }

        [Fact] // +++ no event type in collection code
        public void ItemsChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyResetEvent()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset, new List<string> { CollectionHelper.GroupKeyFirst }));
        }

        [Fact] // +++
        public void ItemsChanged_ClearGroupOnlyAllowEmptyExistKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ClearGroup(CollectionHelper.GroupKeyFirst);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact] // +++
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

        [Fact] // +++
        public void ItemsChanged_InsertGroupsKeysOnlyAllowEmptyGroupUniqueKey_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.KeysNotContained);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ItemsChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ItemsChanged_InsertGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.InsertGroups(0, CollectionHelper.PairNotContainedKeyWithItems);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact] // +++
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

        [Fact] // +++
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

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ItemsChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyReplaceEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Replace, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ItemsChanged_ReplaceGroupsWithItemsCorrectKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.ReplaceGroups(pairs);

            catcher.Unsubscribe();

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            Assert.Throws<InvalidOperationException>(() => collection.AddGroups(CollectionHelper.KeysNull));
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expectedException = new ArgumentNullException(KeysParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.KeysNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupsEmptyListOfKeys_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expected = collection.Keys.Count;

            collection.AddGroups(CollectionHelper.KeysEmpty);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupsNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expectedException = new ArgumentNullException(KeysParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.KeysOneFillOneNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupsUniqueKeys_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysOneFillOneEmpty;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupsDuplicateKeys_ArgumentException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expectedException = new ArgumentException(AddDuplicateItemExceptionMessage);
            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(CollectionHelper.KeysDuplicate));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException(ItemsParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.AddGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact] // +++
        public void AddGroups_WithItemsAllowEmptyGroupsUniqueKeysWithEmptyItems_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.AddGroups(pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact] // +++
        public void AddGroups_WithItemsForbidEmptyGroupsUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            Assert.Throws<InvalidOperationException>(() => collection.AddGroups(CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pair = CollectionHelper.PairDuplicateKeyWithItems;
            var expectedException = new ArgumentException(AddDuplicateItemExceptionMessage);
            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithItems));
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void AddGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithNullItems));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_KeysOnlyNegativeIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(-1, CollectionHelper.KeysNotContained));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_KeysOnlyOverSizedIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(collection.Keys.Count + 1, CollectionHelper.KeysNotContained));
        }

        [Fact] // +++
        public void InsertGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.InsertGroups(0, CollectionHelper.KeysNotContained));
        }

        [Fact] // +++
        public void InsertGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.KeysNull));
        }

        [Fact] // +++
        public void InsertGroups_KeysOnlyAllowEmptyGroupEmptyListOfKeys_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysEmpty;
            var expected = collection.Count + keys.Count;

            collection.InsertGroups(0, keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact] // +++
        public void InsertGroups_KeysOnlyAllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.KeysOneNull));
        }

        [Fact] // +++
        public void InsertGroups_KeysOnlyAllowEmptyGroupDuplicateKey_ArgumentException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var expectedException = new ArgumentException(AddDuplicateItemExceptionMessage);
            var actualException = Assert.Throws<ArgumentException>(() => collection.InsertGroups(0, CollectionHelper.KeysOneFill));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void InsertGroups_KeysOnlyAllowEmptyGroupUniqueKey_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysNotContained;
            var expected = collection.Count + keys.Count;

            collection.InsertGroups(0, keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Theory] // +++ check collection code
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsNegativeIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(-1, CollectionHelper.PairNotContainedKeyWithItems));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsOverSizedIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.InsertGroups(collection.Keys.Count + 1, CollectionHelper.PairNotContainedKeyWithItems));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException(ItemsParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.InsertGroups(0, CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithItems));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithNullItems));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.InsertGroups(0, pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact] // review night
        public void InsertGroups_WithItemsAllowEmptyGroupsUniqueKeysWithEmptyItems_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.InsertGroups(0, pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact] // review night
        public void InsertGroups_WithItemsForbidEmptyGroupsUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.InsertGroups(0, CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pair = CollectionHelper.PairDuplicateKeyWithItems;
            var expectedException = new ArgumentException(AddDuplicateItemExceptionMessage);
            var actualException = Assert.Throws<ArgumentException>(() => collection.InsertGroups(0, pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void InsertGroups_WithItemsDuplicateKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithNullItems));
        }

        [Fact] // review night
        public void InsertGroups_WithItemsAllowEmptyGroupsDuplicateKeysWithEmptyItems_ArgumentException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var expectedException = new ArgumentException(AddDuplicateItemExceptionMessage);
            var actualException = Assert.Throws<ArgumentException>(() => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithEmptyItem));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // review night
        public void InsertGroups_WithItemsForbidEmptyGroupsDuplicateKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithEmptyItem));
        }

        [Fact] // review night
        public void ReplaceGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.ReplaceGroups(CollectionHelper.KeysNotContained));
        }

        [Fact] // review night
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.KeysNull));
        }

        [Fact] // review night
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupEmptyListOfKeys_CollectionIsEmpty()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            collection.ReplaceGroups(CollectionHelper.KeysEmpty);

            Assert.Equal(0, collection.Keys.Count);
        }

        [Fact] // review night
        public void ReplaceGroups_KeysOnlyAllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.KeysOneNull));
        }

        [Fact] // +++
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

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void ReplaceGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException(ItemsParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void ReplaceGroups_WithItemsEmptyListOfPairs_CollectionIsEmpty(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ReplaceGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(0, collection.Count);
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void ReplaceGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairNullKeyWithItems));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void ReplaceGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory] // review night
        [MemberData(nameof(FillCollectionOptions))]
        public void ReplaceGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairNullKeyWithNullItems));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
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

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void ReplaceGroups_WithItemsCorrectKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceGroups(CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact] // +++
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

        [Fact] // +++
        public void ReplaceGroups_WithItemsForbidEmptyGroupsCorrectKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.ReplaceGroups(CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void RemoveGroups_NullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.RemoveGroups(CollectionHelper.KeysNull));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void RemoveGroups_ContainsKey_CollectionSizeDecreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysOneFill;
            var expectedCount = collection.Count - keys.Count;

            collection.RemoveGroups(keys);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void RemoveGroups_MissingKey_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.RemoveGroups(CollectionHelper.KeysNotContained));
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void RemoveGroups_ContainsAndMissKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.RemoveGroups(CollectionHelper.KeysOneContainedOneNotContained));
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void ClearGroups_EmptyCollection_CollectionEmpty(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ClearGroups();

            Assert.Equal(0, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
        public void ClearGroups_FilledCollection_CollectionCleared(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ClearGroups();

            Assert.Equal(0, collection.Count);
        }

        [Fact] // +++
        public void ClearGroup_ForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(() => collection.ClearGroup(CollectionHelper.GroupKeyFirst));
        }

        [Fact] // +++
        public void ClearGroup_AllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(() => collection.ClearGroup(null));
        }

        [Fact] // +++
        public void ClearGroup_AllowEmptyGroupExistKey_ItemsRemoved()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var key = CollectionHelper.GroupKeyFirst;
            var startCount = collection.FirstOrDefault(x => x.Key == key)?.Count() ?? 0;

            collection.ClearGroup(key);

            var currentCount = collection.FirstOrDefault(x => x.Key == key)?.Count() ?? -1;

            Assert.True((startCount > 0) && currentCount == 0);
        }

        [Fact] // +++
        public void ClearGroup_AllowEmptyGroupMissingKey_KeyNotFoundException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<KeyNotFoundException>(() => collection.ClearGroup(CollectionHelper.GroupKeyThird));
        }

        [Fact]
        // AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        public void _AddItemsWithSelectors_Test_Miss()
        {
            Assert.True(false);

            var collection = CollectionHelper.CreateWithEmptyGroups();
            collection.AddItems();
        }

        [Fact]
        // InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector)
        public void _InsertItemsWithSelectors_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // ReplaceItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        public void _ReplaceItemsWithSelectors_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        public void _RemoveItemsWithSelectors_Test_Miss()
        {
            Assert.True(false);
        }

        [Theory] // +++
        [MemberData(nameof(EmptyCollectionOptions))]
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
