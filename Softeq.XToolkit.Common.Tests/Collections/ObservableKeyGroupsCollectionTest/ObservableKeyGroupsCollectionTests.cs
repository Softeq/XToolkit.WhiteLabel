// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest;
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
                new object[] {CollectionHelper.CreateWithoutEmptyGroups() }
            };

        public static IEnumerable<object[]> FillCollectionOptions =>
         new List<object[]>
         {
                new object[] {CollectionHelper.CreateWithItemsWithEmptyGroups() },
                new object[] {CollectionHelper.CreateWithItemsWithoutEmptyGroups() }
         };

        [Fact]
        // event NotifyCollectionChangedEventHandler CollectionChanged;
        public void CollectionChanged_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact] // +++
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact] // +++
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void CollectionChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void CollectionChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        // event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;
        public void ItemsChanged_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact] // +++
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact] // +++
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_NotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            Assert.Equal(1, catcher.EventCount);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void ItemChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyAddEvent(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void ItemChanged_AddGroupsWithItemsUniqueKeysWithItems_NotifyOneTime(ObservableKeyGroupsCollection<string, int> collection)
        {
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.PairsWithKeysWithItems);

            Assert.Equal(1, catcher.EventCount);
        }

        // AddGroups(keys)
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

        [Fact] // +++
        public void AddGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var exception = new InvalidOperationException();
            var actual = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(CollectionHelper.KeysNull));

            Assert.Equal(exception.Message, actual.Message);
        }

        // AddGroups(keys, items)
        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException(ItemsParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.AddGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairWithKeyWithNullItems));

            Assert.Equal(expectedException.Message, actualException.Message);
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
            var expectedException = new InvalidOperationException();
            var actualException = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(CollectionHelper.PairWithKeyWithEmptyItem));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pair = CollectionHelper.PairDuplicateKeyWithItems;
            var expectedException = new ArgumentException(AddDuplicateItemExceptionMessage);
            collection.AddGroups(pair);
            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithItems));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithEmptyItems));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Theory] // +++
        [MemberData(nameof(CollectionOptions))]
        public void AddGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairNullKeyWithNullItems));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        // InsertGroups(int index, IEnumerable<TKey> keys)
        public void InsertGroups_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // InsertGroups(int index, IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        public void InsertGroupsParams_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // ReplaceGroups(IEnumerable<TKey> keys)
        public void ReplaceGroups_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // ReplaceGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        public void ReplaceGroupsParams_Test_Miss()
        {
            Assert.True(false);
        }

        [Theory] // +++
        [MemberData(nameof(FillCollectionOptions))]
        public void RemoveGroups_NullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new ArgumentNullException(KeysParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.RemoveGroups(CollectionHelper.KeysNull));

            Assert.Equal(expectedException.Message, actualException.Message);
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
        public void RemoveGroups_MissKey_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedException = new KeyNotFoundException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.RemoveGroups(CollectionHelper.KeysNotContained));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        #region ClearGroups
        //[Fact]
        //public void ClearGroups_EmptyCollectionForAllowedEmptyGroups_CollectionCleared()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    collection.ClearGroups();

        //    Assert.Equal(0, collection.Count);
        //}

        //[Fact]
        //public void ClearGroups_EmptyCollectionForForbiddenEmptyGroups_CollectionCleared()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithoutEmptyGroups();
        //    collection.ClearGroups();

        //    Assert.Equal(0, collection.Count);
        //}

        //[Fact]
        //public void ClearGroups_FilledCollectionForAllowedEmptyGroups_CollectionCleared()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
        //    collection.ClearGroups();

        //    Assert.Equal(0, collection.Count);
        //}

        //[Fact]
        //public void ClearGroups_FilledCollectionForForbiddenEmptyGroups_CollectionCleared()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithoutEmptyGroups();
        //    collection.ClearGroups();

        //    Assert.Equal(0, collection.Count);
        //}

        //[Fact]
        //public void ClearGroups_FilledCollection_CollectionChangedNotifyOneTime()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateCollectionEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.ClearGroups();

        //    Assert.Equal(1, catcher.EventCount);
        //}

        //[Fact]
        //public void ClearGroups_FilledCollection_CollectionChangedNotifyResetEvent()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateCollectionEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.ClearGroups();

        //    Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        //}

        //[Fact]
        //public void ClearGroups_EmptyCollection_CollectionChangedNotifyResetEvent()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateCollectionEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.ClearGroups();

        //    Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        //}

        //[Fact]
        //public void ClearGroups_FilledCollection_ItemsChangedNotifyOneTime()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateItemsEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.ClearGroups();

        //    Assert.Equal(1, catcher.EventCount);
        //}

        //[Fact]
        //public void ClearGroups_FilledCollection_ItemsChangedNotifyAddEvent()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateItemsEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.ClearGroups();

        //    Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        //}

        //[Fact]
        //public void ClearGroups_EmptyCollection_ItemsChangedNotifyAddEvent()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateItemsEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.ClearGroups();

        //    Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Reset));
        //}

        #endregion

        [Fact]
        // ClearGroup(TKey key)
        public void ClearGroup_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // AddItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        public void AddItems_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // InsertItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector, Func<T, int> valueIndexSelector)
        public void InsertItems_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // ReplaceItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        public void ReplaceItems_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // RemoveItems<T>(IEnumerable<T> items, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
        public void RemoveItems_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact] // +++
        public void GetEnumerator_AllowEmptyGroups_AllElementsPassed()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
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

        [Fact] // +++
        public void GetEnumerator_ForbidEmptyGroups_AllElementsPassed()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
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