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
    ///     Tests for ICollection interface
    /// </summary>
    public class CollectionInterfaceTests
    {
        [Fact]
        public void AddTest()
        {
            var testData = new TestData();
            var collection = testData.CollectionInterface;

            collection.Add(new KeyValuePair<int, string>(0, "1"));

            Assert.Equal("[0, 1]-[1, 0]", testData.ExtractResult());
        }

        [Fact]
        public void AddWithExceptionsTest()
        {
            var testData = new TestData(3);
            var collection = testData.CollectionInterface;

            var ex =
                Assert.Throws<ArgumentException>(() => collection.Add(new KeyValuePair<int, string>(1, "1")));
            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void ClearTest()
        {
            var testData = new TestData(3);
            var collection = testData.CollectionInterface;

            collection.Clear();

            var dictionary = testData.Dictionary;
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void ContainsTest()
        {
            var testData = new TestData(2);
            var collection = testData.CollectionInterface;

            Assert.Contains(collection.FirstOrDefault(), collection);
            Assert.DoesNotContain(new KeyValuePair<int, string>(2, "2"), collection);
        }

        [Fact]
        public void EnumerateTest()
        {
            var testData = new TestData(2);
            var dictionary = testData.CollectionInterface;

            var sb = new StringBuilder();
            sb.AppendJoin(",", dictionary);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void RemoveTest()
        {
            var testData = new TestData(2);
            var collection = testData.CollectionInterface;

            var item = collection.ElementAt(0);

            var wasRemovedFromCollection = collection.Remove(item);
            var wasNotRemovedFromCollection = collection.Remove(item);

            Assert.True(wasRemovedFromCollection);
            Assert.False(wasNotRemovedFromCollection);
            Assert.Equal("[1, 1]-[0, 0],[1, 1]", testData.ExtractResult());
        }

        //reverse

        [Fact]
        public void ReverseAddTest()
        {
            var testData = new ReverseTestData();
            var collection = testData.ReverseCollectionInterface;

            collection.Add(new KeyValuePair<int, string>(0, "1"));

            Assert.Equal("[1, 0]-[0, 1]", testData.ExtractResult());
        }

        [Fact]
        public void ReverseAddWithExceptionsTest()
        {
            var testData = new ReverseTestData(3);
            var collection = testData.ReverseCollectionInterface;

            var ex =
                Assert.Throws<ArgumentException>(() => collection.Add(new KeyValuePair<int, string>(1, "1")));
            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void ReverseClearTest()
        {
            var testData = new ReverseTestData(3);
            var collection = testData.ReverseCollectionInterface;

            collection.Clear();

            var dictionary = testData.Dictionary;
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void ReverseContainsTest()
        {
            var testData = new ReverseTestData(2);
            var collection = testData.ReverseCollectionInterface;

            Assert.Contains(collection.FirstOrDefault(), collection);
            Assert.DoesNotContain(new KeyValuePair<int, string>(2, "2"), collection);
        }

        [Fact]
        public void ReverseEnumerateTest()
        {
            var testData = new ReverseTestData(2);
            var collection = testData.ReverseCollectionInterface;

            var sb = new StringBuilder();
            sb.AppendJoin(",", collection);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void ReverseRemoveTest()
        {
            var testData = new ReverseTestData(2);
            var collection = testData.ReverseCollectionInterface;

            var item = collection.ElementAt(0);

            var wasRemovedFromCollection = collection.Remove(item);
            var wasNotRemovedFromCollection = collection.Remove(item);

            Assert.True(wasRemovedFromCollection);
            Assert.False(wasNotRemovedFromCollection);
            Assert.Equal("[0, 0],[1, 1]-[1, 1]", testData.ExtractResult());
        }
    }
}
