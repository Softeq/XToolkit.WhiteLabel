// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest;
using Xunit;

using Helper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace ObservableKeyGroupsCollection
{
    public class ObservableKeyGroupsCollectionTests
    {
        [Fact]
        // event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;
        public void ItemsChanged_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // event NotifyCollectionChangedEventHandler CollectionChanged;
        public void CollectionChanged_Test_Miss()
        {
            Assert.True(false);
        }

        [Fact]
        // ObservableKeyGroupsCollections(bool withoutEmptyGroups = true)
        public void Constructor_Test_Miss()
        {
            Assert.True(false);
        }

        #region AddGroups_WithoutItems
        [Fact]
        public void AddGroups_WithOutItemsWithoutEmptyGroupsNullListOfKeys_InvalidOperationException()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var keys = Helper.KeysNull;
            var exception = new InvalidOperationException();

            var actual = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(keys));

            Assert.Equal(exception.Message, actual.Message);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithoutEmptyGroupsKeysWithoutItems_InvalidOperationException()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var keys = Helper.KeysOneFillOneEmpty;
            var expectedException = new InvalidOperationException();

            var actualException = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(keys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsNullListOfKeys_ArgumentNullException()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var keys = Helper.KeysNull;
            var expectedException = new ArgumentNullException(Helper.KeysParameterName);

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(keys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsEmptyListOfKeys_KeysCountNotChanged()
        {
            var collection = Helper.CreateWithItemsWithEmptyGroups();
            var keys = Helper.KeysEmpty;
            var expected = collection.Keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsOneNullKey_ArgumentNullException()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var keys = Helper.KeysNull;
            var expectedException = new ArgumentNullException(Helper.KeysParameterName);

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(keys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsUniqueKeys_KeyCountIncreaseByNumberOfUniqueKeys()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var keys = Helper.KeysTwoFill;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsUniqueEmptyKey_KeyCountIncreaseByNumberOfUniqueKeys()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var keys = Helper.KeysOneEmpty;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsDuplicateKeys_ArgumentException()
        {
            var collection = Helper.CreateWithItemsWithEmptyGroups();
            var expectedException = new ArgumentException();

            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(Helper.KeysOneFill));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsUniqueKeys_CollectionChangedNotifyAddEvent()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var keys = Helper.KeysTwoFill;
            var catcher = Helper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsUniqueKeys_CollectionChangedNotifyOneTime()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var catcher = Helper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(Helper.KeysTwoFill);

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsUniqueKeys_ItemsChangedNotifyAddEvent()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var keys = Helper.KeysTwoFill;
            var catcher = Helper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        }

        [Fact]
        public void AddGroups_WithoutItemsWithEmptyGroupsUniqueKeys_ItemsChangedNotifyOneTime()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var catcher = Helper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(Helper.KeysTwoFill);

            Assert.Equal(1, catcher.EventCount);
        }
        #endregion

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullLitsOfPairs_ArgumentNullException()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var expectedException = new ArgumentNullException(Helper.ItemsParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(Helper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullLitsOfPairs_ArgumentNullException()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var expectedException = new ArgumentNullException(Helper.ItemsParameterName);
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(Helper.PairsNull));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForEmptyLitsOfPairs_CollectionSizeNotChanged()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var expectedCount = collection.Count;

            collection.AddGroups(Helper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForEmptyLitsOfPairs_CollectionSizeNotChanged()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var expectedCount = collection.Count;

            collection.AddGroups(Helper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForUniqueKeysWithItems_CollectionSizeIncreasedByPairsNumber()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForUniqueKeysWithItems_CollectionSizeIncreasedByPairsNumber()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForUniqueKeysWithNullItems_ArgumentNullException()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pair = Helper.PairWithKeyWithNullItems;
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForUniqueKeysWithNullItems_ArgumentNullException()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var pair = Helper.PairWithKeyWithNullItems;
            var expectedException = new ArgumentNullException();
            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForUniqueKeysWithEmptyItems_CollectionSizeIncreasedByPairsNumber()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pair = Helper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.AddGroups(pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var pair = Helper.PairWithKeyWithEmptyItem;
            var expectedException = new InvalidOperationException();

            var actualException = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        // over
        //[Fact]
        //public void AddGroups_WithItemsWithEmptyGroupsForDuplicateKeyWithItems_ArgumentException()
        //{
        //    var collection = Helper.CreateWithItemsWithEmptyGroups();
        //    var pair = Helper.PairDuplicateKeyWithItems;
        //    var expectedException = new ArgumentException();

        //    var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

        //    Assert.Equal(expectedException.Message, actualException.Message);
        //}

        // over
        //[Fact]
        //public void AddGroups_WithItemsWithoutEmptyGroupsForDuplicateKeyWithItems_ArgumentException()
        //{
        //    var collection = Helper.CreateWithItemsWithoutEmptyGroups();
        //    var pair = Helper.PairDuplicateKeyWithItems;
        //    var expectedException = new ArgumentException();

        //    var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

        //    Assert.Equal(expectedException.Message, actualException.Message);
        //}

        // over
        //[Fact]
        //public void AddGroups_WithItemsWithEmptyGroupsForDuplicateKeyWithNullItems_ArgumentException()
        //{
        //    var collection = Helper.CreateWithItemsWithEmptyGroups();
        //    var pair = Helper.PairDuplicateKeyWithNullItems;
        //    var expectedException = new ArgumentException();

        //    var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

        //    Assert.Equal(expectedException.Message, actualException.Message);
        //}

        // over
        //[Fact]
        //public void AddGroups_WithItemsWithoutEmptyGroupsForDuplicateKeyWithNullItems_ArgumentException()
        //{
        //    var collection = Helper.CreateWithItemsWithoutEmptyGroups();
        //    var pair = Helper.PairDuplicateKeyWithNullItems;
        //    var expectedException = new ArgumentException();

        //    var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

        //    Assert.Equal(expectedException.Message, actualException.Message);
        //}

        //[InlineData(null, new List())]
        //[Fact]
        //public void AddGroups_WithItemsWithEmptyGroupsForDuplicateKeyWithAnyItems_ArgumentException()
        //{
        //    var collection = Helper.CreateWithItemsWithEmptyGroups();
        //    var pair = Helper.PairDuplicateKeyWithEmptyItem;
        //    var expectedException = new ArgumentException();

        //    var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

        //    Assert.Equal(expectedException.Message, actualException.Message);
        //}

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForDuplicateKeyWithAnyItems_ArgumentException()
        {
            var collection = Helper.CreateWithItemsWithoutEmptyGroups();
            var pair = Helper.PairDuplicateKeyWithEmptyItem;
            var expectedException = new ArgumentException();

            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullKeyWithItems_ArgumentNullException()
        {
            var collection = Helper.CreateWithItemsWithEmptyGroups();
            var pair = Helper.PairNullKeyWithItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullKeyWithItems_ArgumentNullException()
        {
            var collection = Helper.CreateWithItemsWithoutEmptyGroups();
            var pair = Helper.PairNullKeyWithItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullKeyWithEmptyItems_ArgumentNullException()
        {
            var collection = Helper.CreateWithItemsWithEmptyGroups();
            var pair = Helper.PairNullKeyWithEmptyItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullKeyWithEmptyItems_ArgumentNullException()
        {
            var collection = Helper.CreateWithItemsWithoutEmptyGroups();
            var pair = Helper.PairNullKeyWithEmptyItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsForNullKeyWithNullItems_ArgumentNullException()
        {
            var collection = Helper.CreateWithItemsWithEmptyGroups();
            var pair = Helper.PairNullKeyWithNullItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithoutEmptyGroupsForNullKeyWithNullItems_ArgumentNullException()
        {
            var collection = Helper.CreateWithItemsWithoutEmptyGroups();
            var pair = Helper.PairNullKeyWithNullItems;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(pair));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_CollectionChangedNotifyAddEvent()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
            var catcher = Helper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_CollectionChangedNotifyOneTime()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
            var catcher = Helper.CreateCollectionEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.Equal(1, catcher.EventCount);
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_ItemsChangedNotifyAddEvent()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
            var catcher = Helper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, pairs));
        }

        [Fact]
        public void AddGroups_WithItemsWithEmptyGroupsUniqueKeysWithItems_ItemsChangedNotifyOneTime()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
            var catcher = Helper.CreateItemsEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(pairs);

            Assert.Equal(1, catcher.EventCount);
        }

        //[Fact]
        //public void AddGroups_UniqueKeysForAllowedEmptyGroups_CollectionChangedNotifyOneTime()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateCollectionEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.AddGroups(ObservableKeyGroupsCollectionHelper.TwoGroupKeys);

        //    Assert.Equal(1, catcher.EventCount);
        //}

        //[Fact]
        //public void AddGroups_UniqueKeysForAllowedEmptyGroups_ItemsChangedNotifyAddEvent()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    var keys = ObservableKeyGroupsCollectionHelper.TwoGroupKeys;
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateItemsEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.AddGroups(keys);

        //    Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, keys));
        //}

        //[Fact]
        //public void AddGroups_UniqueKeysForAllowedEmptyGroups_ItemsChangedNotifyOneTime()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    var catcher = ObservableKeyGroupsCollectionHelper.CreateItemsEventCatcher(collection);
        //    catcher.Subscribe();

        //    collection.AddGroups(ObservableKeyGroupsCollectionHelper.TwoGroupKeys);

        //    Assert.Equal(1, catcher.EventCount);
        //}


        // ----------

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

        [Fact]
        public void GetEnumerator_WithEmptyGroups()
        {
            var collection = Helper.CreateEmptyWithEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItemsWithEmpty;
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

        [Fact]
        public void GetEnumerator_WithoutEmptyGroups()
        {
            var collection = Helper.CreateEmptyWithoutEmptyGroups();
            var pairs = Helper.PairsWithKeysWithItems;
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