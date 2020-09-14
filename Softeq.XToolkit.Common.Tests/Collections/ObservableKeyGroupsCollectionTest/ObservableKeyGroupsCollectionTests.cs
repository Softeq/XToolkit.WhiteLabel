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
        [Fact]
        // event NotifyCollectionChangedEventHandler CollectionChanged;
        public void CollectionChanged_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact] // +++
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_CollectionChangedNotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            var keys = CollectionHelper.KeysTwoFill;

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact] // +++
        public void CollectionChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_CollectionChangedNotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        // event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;
        public void ItemsChanged_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact] // +++
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_ItemsChangedNotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysTwoFill;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact] // +++
        public void ItemsChanged_AddGroupsKeysOnlyAllowEmptyGroupsUniqueKeys_ItemsChangedNotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(CollectionHelper.KeysTwoFill);

            Assert.Equal(1, catcher.EventCount);
        }

        // AddGroups(keys)
        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expectedException = new ArgumentNullException(CollectionHelper.KeysParameterName); // how can we use real variable name?
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.KeysNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupsEmptyListOfKeys_KeysCountNotChanged()
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
            var expectedException = new ArgumentNullException(CollectionHelper.KeysParameterName); // how can we use real variable name?
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.KeysOneFillOneNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyAllowEmptyGroupsUniqueKeys_KeyCountIncreaseByNumberOfUniqueKeys()
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
            var expectedException = new ArgumentException("An item with the same key has already been added."); // should solve message
            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(CollectionHelper.KeysDuplicate));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void AddGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var keys = CollectionHelper.KeysNull;
            var exception = new InvalidOperationException();
            var actual = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(keys));

            Assert.Equal(exception.Message, actual.Message);
        }

        // AddGroups(keys, items)
        [Fact] // +++
        public void AddGroups_WithItemsAllowEmptyGroupsNullListOfPairs_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expectedException = new ArgumentNullException(CollectionHelper.ItemsParameterName); // how can we use real variable name?
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact] // +++
        public void AddGroups_WithItemsForbidEmptyGroupsNullListOfPairs_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var expectedException = new ArgumentNullException(CollectionHelper.ItemsParameterName); // how can we use real variable name?
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(CollectionHelper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForEmptyListOfPairs_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expectedCount = collection.Count;

            collection.AddGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForEmptyListOfPairs_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var expectedCount = collection.Count;

            collection.AddGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForUniqueKeysWithItems_CollectionSizeIncreasedByPairsNumber()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForUniqueKeysWithItems_CollectionSizeIncreasedByPairsNumber()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForUniqueKeysWithNullItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithNullItems;
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForUniqueKeysWithNullItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithNullItems;
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForUniqueKeysWithEmptyItems_CollectionSizeIncreasedByPairsNumber()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.AddGroups(pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedException = new InvalidOperationException();

            var actualException = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForDuplicateKeyWithAnyItems_ArgumentException()
        {
            var collection = CollectionHelper.CreateWithItemsWithoutEmptyGroups();
            var pair = CollectionHelper.PairDuplicateKeyWithEmptyItem;
            var expectedException = new ArgumentException();

            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullKeyWithItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithItemsWithEmptyGroups();
            var pair = CollectionHelper.PairNullKeyWithItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullKeyWithItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithItemsWithoutEmptyGroups();
            var pair = CollectionHelper.PairNullKeyWithItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullKeyWithEmptyItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithItemsWithEmptyGroups();
            var pair = CollectionHelper.PairNullKeyWithEmptyItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullKeyWithEmptyItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithItemsWithoutEmptyGroups();
            var pair = CollectionHelper.PairNullKeyWithEmptyItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullKeyWithNullItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithItemsWithEmptyGroups();
            var pair = CollectionHelper.PairNullKeyWithNullItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullKeyWithNullItems_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithItemsWithoutEmptyGroups();
            var pair = CollectionHelper.PairNullKeyWithNullItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_CollectionChangedNotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_CollectionChangedNotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_ItemsChangedNotifyAddEvent()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_ItemsChangedNotifyOneTime()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var catcher = CollectionHelper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.Equal(1, catcher.EventCount);
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

        [Fact]
        // RemoveGroups(IEnumerable<TKey> keys)
        public void RemoveGroups_Test_Miss()
        {
            Assert.True(false);
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