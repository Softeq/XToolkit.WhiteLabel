// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionaryTests
{
    /// <summary>
    ///     Tests for ICollection interface.
    /// </summary>
    public class CollectionInterfaceTests
    {
        [Fact]
        public void Add()
        {
            var collection = BiDictionaryHelper.CreateEmpty().ToICollection();

            collection.Add(new KeyValuePair<int, string>(0, "1"));

            Assert.Equal("[0, 1]-[1, 0]", collection.GetResult());
        }

        [Fact]
        public void Add_ThrowsArgumentException()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            var ex =
                Assert.Throws<ArgumentException>(() => collection.Add(new KeyValuePair<int, string>(1, "1")));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void Clear()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            collection.Clear();

            Assert.Empty(collection);
        }

        [Fact]
        public void Contains()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            Assert.Contains(collection.FirstOrDefault(), collection);
        }

        [Fact]
        public void Contains_Negative()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            Assert.DoesNotContain(new KeyValuePair<int, string>(2, "2"), collection);
        }

        [Fact]
        public void Enumerate()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            var sb = new StringBuilder();
            sb.AppendJoin(",", collection);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void Remove()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            var item = collection.ElementAt(0);
            var wasRemovedFromCollection = collection.Remove(item);

            Assert.True(wasRemovedFromCollection);
            Assert.Equal("[1, 1]-[0, 0],[1, 1]", collection.GetResult());
        }

        [Fact]
        public void Remove_Negative()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItems().ToICollection();

            var item = new KeyValuePair<int, string>(2, "2");
            var wasNotRemovedFromCollection = collection.Remove(item);

            Assert.False(wasNotRemovedFromCollection);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", collection.GetResult());
        }

        [Fact]
        public void Add_Reverse()
        {
            var original = BiDictionaryHelper.CreateEmptyReverse();
            var collection = original.Reverse.ToICollection();

            collection.Add(new KeyValuePair<int, string>(0, "1"));

            Assert.Equal("[1, 0]-[0, 1]", original.GetResult());
        }

        [Fact]
        public void Add_ThrowsArgumentException_Reverse()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToICollection();

            var ex =
                Assert.Throws<ArgumentException>(() => collection.Add(new KeyValuePair<int, string>(1, "1")));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void Clear_Reverse()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToICollection();

            collection.Clear();

            Assert.Empty(collection);
        }

        [Fact]
        public void Contains_Reverse()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToICollection();

            Assert.Contains(collection.FirstOrDefault(), collection);
        }

        [Fact]
        public void Contains_Negative_Reverse()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToICollection();

            Assert.DoesNotContain(new KeyValuePair<int, string>(2, "2"), collection);
        }

        [Fact]
        public void Enumerate_Reverse()
        {
            var collection = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToICollection();

            var sb = new StringBuilder();
            sb.AppendJoin(",", collection);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void Remove_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var collection = original.Reverse.ToICollection();

            var item = collection.ElementAt(0);
            var wasRemovedFromCollection = collection.Remove(item);

            Assert.True(wasRemovedFromCollection);
            Assert.Equal("[0, 0],[1, 1]-[1, 1]", original.GetResult());
        }

        [Fact]
        public void Remove_Negative_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var collection = original.Reverse.ToICollection();

            var item = new KeyValuePair<int, string>(2, "2");
            var wasNotRemovedFromCollection = collection.Remove(item);

            Assert.False(wasNotRemovedFromCollection);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", original.GetResult());
        }
    }
}
