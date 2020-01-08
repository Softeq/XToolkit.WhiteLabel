using System;
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

        #region 
        [Fact]
        // AddGroups(IEnumerable<TKey> keys)
        public void AddGroups_Test_Miss()
        {
            // + null keys 
            // + empty keys
            // one key
            // duplicate one key
            // several keys
            // duplicate one key in several
            // duplicate all keys in several
            // nptification change collection several items
            // no notification change items sevela items
            // exceptions?

            Assert.True(false);
        }

        [Fact]
        public void AddGroups_NullKeysAllowedEmptyGroup_ArgumentNullException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.NullKeys;
            var argumentException = new ArgumentNullException();

            var actual = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(keys));

            Assert.Equal(argumentException.Message, actual.Message);
        }

        [Fact]
        public void AddGroups_NullKeysForbiddenEmptyGroup_ArgumentNullException()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithoutEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.NullKeys;
            var argumentException = new ArgumentNullException();

            var actual = Assert.Throws<ArgumentNullException>(() => collection.AddGroups(keys));

            Assert.Equal(argumentException.Message, actual.Message);
        }

        [Fact]
        public void AddGroups_EmptyKeysAllowedEmptyGroup_GroupCountNotChanged()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.EmptyKeys;

            collection.AddGroups(keys);

            Assert.False(true);
        }

        [Fact]
        public void AddGroups_EmptyKeysForbiddenEmptyGroup_GroupCountNotChanged()
        {
            var collection = ObservableKeyGroupsCollectionHelper.CreateEmptyWithoutEmptyGroups();
            var keys = ObservableKeyGroupsCollectionHelper.EmptyKeys;

            collection.AddGroups(keys);

            Assert.False(true);
        }



        #endregion

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
        // IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        public void GetEnumerator_Test_Miss()
        {
            Assert.True(false);
        }
    }
}