using System;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollection
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

        [Fact]
        public void AddGroups_NullListOfKeysForForbiddenEmptyGroups_InvalidOperationException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithoutEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.NullKeys;
            var exception = new InvalidOperationException();

            var actual = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(keys));

            Assert.Equal(exception.Message, actual.Message);
        }

        [Fact]
        public void AddGroups_KeysWithoutItemsForForbiddenEmptyGroups_InvalidOperationException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithoutEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.TwoWithEmptyGroupKeys;
            var expectedException = new InvalidOperationException();

            var actualException = Assert.Throws<InvalidOperationException>(() => collection.AddGroups(keys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_NullListOfKeysForAllowedEmptyGroups_ArgumentNullException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.NullKeys;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(keys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_EmptyListOfKeysForAllowedEmptyGroups_KeysCountNotChanged()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
            var expected = collection.Keys.Count;
            var keys = ObservableKeyGroupsCollectionHelper.EmptyKeys;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_OneNullKeyForAllowedEmptyGroups_ArgumentNullException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.OneNullGroupKeys;
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(keys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_UniqueKeysForAllowedEmptyGroups_KeyCountIncreaseByNumberOfUniqueKeys()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.TwoGroupKeys;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_UniqueEmptyKeyForAllowedEmptyGroups_KeyCountIncreaseByNumberOfUniqueKeys()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.OneEmptyGroupKeys;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_DuplicateKeysForAllowedEmptyGroups_ArgumentException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Throws<ArgumentException>(() => collection.AddGroups(ObservableKeyGroupsCollectionHelper.OneGroupKeys));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public void AddGroups_UniqueKeysForAllowedEmptyGroups_CollectionChangedNotifyOneTime()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.TwoGroupKeys;
            var catcher = ObservableKeyGroupsCollectionHelper.CreateEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(keys);

            Assert.True(catcher.IsExpectedEvent(NotifyCollectionChangedAction.Add, ObservableKeyGroupsCollectionHelper.TwoWithEmptyGroupKeys));
        }

        [Fact]
        public void AddGroups_UniqueKeysForAllowedEmptyGroups_CollectionChangedNotifyAddEvent()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var catcher = ObservableKeyGroupsCollectionHelper.CreateEventCatcher(collection);
            catcher.Subscribe();

            collection.AddGroups(ObservableKeyGroupsCollectionHelper.TwoGroupKeys);

            Assert.True(false);
        }


        //public event EventHandler<NotifyKeyGroupCollectionChangedEventArgs<TKey, TValue>> ItemsChanged;

        //[Fact]
        //public void AddGroups_TwoKeysForAllowedEmptyGroups_ItemsChangedForTwoKeys()
        //{
        //    var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
        //    var handler = ObservableKeyGroupsCollectionHelper.CreateEventCatcher(collection);
        //    collection.ItemsChanged += handler.EventHandler;

        //    collection.AddGroups(ObservableKeyGroupsCollectionHelper.TwoGroupKeys);

        //    Assert.Equal(ObservableKeyGroupsCollectionHelper.TwoGroupKeys.Count, handler.Count);
        //}

        [Fact]
        // AddGroups(IEnumerable<KeyValuePair<TKey, IList<TValue>>> items)
        public void AddGroupsParams_Test_Miss()
        {
            Assert.True(false);
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

        [Fact]
        // ClearGroups()
        public void ClearGroups_Test_Miss()
        {
            Assert.True(false);
        }

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
            var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithEmptyGroups();

            //Console.WriteLine(collection.GetEnumerator());

            Assert.True(false);
        }

        [Fact]
        public void GetEnumerator_WithoutEmptyGroups()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateWithItemsWithoutEmptyGroups();
            //Console.WriteLine(collection.GetEnumerator());

            Assert.True(false);
        }
    }
}