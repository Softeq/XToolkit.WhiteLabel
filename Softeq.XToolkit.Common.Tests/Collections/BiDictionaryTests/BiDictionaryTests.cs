// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Softeq.XToolkit.Common.Collections;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionaryTests
{
    public class BiDictionaryTests
    {
        [Fact]
        public void AddTest()
        {
            var testData = new TestData();

            var dictionary = testData.Dictionary;
            dictionary.Add(0, "0");
            var added = dictionary.TryAdd(1, "1");
            var notAdded = dictionary.TryAdd(1, "1");

            Assert.True(added);
            Assert.False(notAdded);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", testData.ExtractResult());
        }

        [Fact]
        public void AddWithExceptionsTest()
        {
            var testData = new TestData(3);
            var dictionary = testData.Dictionary;

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void ClearTest()
        {
            var testData = new TestData(3);
            var dictionary = testData.Dictionary;

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void ContainsTest()
        {
            var testData = new TestData(2);
            var dictionary = testData.Dictionary;

            Assert.True(dictionary.ContainsKey(0));
            Assert.False(dictionary.ContainsKey(2));
        }

        [Fact]
        public void EnumerateTest()
        {
            var testData = new TestData(2);
            var dictionary = testData.Dictionary;

            var sb = new StringBuilder();
            sb.AppendJoin(",", dictionary);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void GetTests()
        {
            var testData = new TestData(2);
            var dictionary = testData.Dictionary;

            Assert.Equal("0", dictionary[0]);
            Assert.True(dictionary.TryGetValue(0, out var value0));
            Assert.Equal("0", value0);

            Assert.False(dictionary.TryGetValue(2, out var value1));
            Assert.Null(value1);

            var ex = Assert.Throws<KeyNotFoundException>(() => dictionary[2]);
            Assert.Equal("The given key '2' was not present in the dictionary.", ex.Message);
        }

        [Fact]
        public void RemoveTest()
        {
            var testData = new TestData(3);
            var dictionary = testData.Dictionary;

            var wasRemoved0 = dictionary.Remove(0);
            var wasNotRemoved0 = dictionary.Remove(0);

            var wasRemoved1 = dictionary.Remove(1, out var removedValue);
            var wasNotRemoved1 = dictionary.Remove(1, out var notRemovedValue);

            Assert.True(wasRemoved0);
            Assert.False(wasNotRemoved0);
            Assert.True(wasRemoved1);
            Assert.False(wasNotRemoved1);
            Assert.Equal("1", removedValue);
            Assert.Null(notRemovedValue);
            Assert.Equal("[2, 2]-[2, 2]", testData.ExtractResult());
        }

        [Fact]
        public void SerializationTest()
        {
            var testData = new TestData(2);
            var dictionary = testData.Dictionary;

            var sb = new StringBuilder();

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, dictionary);
                memoryStream.Position = 0;

                var newDict = (BiDictionary<int, string>) binaryFormatter.Deserialize(memoryStream);

                sb.AppendJoin(",", newDict);
                sb.Append("-");
                sb.AppendJoin(",", newDict.Reverse);
            }

            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void SetTest()
        {
            var testData = new TestData();
            var dictionary = testData.Dictionary;

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[1, 2]-[0, 0],[2, 1]", testData.ExtractResult());
        }

        //reverse

        [Fact]
        public void ReverseAddTest()
        {
            var testData = new ReverseTestData();
            var dictionary = testData.Dictionary.Reverse;

            dictionary.Add(0, "0");
            var added = dictionary.TryAdd(1, "1");
            var notAdded = dictionary.TryAdd(1, "1");

            Assert.True(added);
            Assert.False(notAdded);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", testData.ExtractResult());
        }

        [Fact]
        public void ReverseAddWithExceptionsTest()
        {
            var testData = new ReverseTestData(3);
            var dictionary = testData.Dictionary.Reverse;

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void ReverseClearTest()
        {
            var testData = new ReverseTestData(3);
            var dictionary = testData.Dictionary.Reverse;

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void ReverseContainsTest()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.Dictionary.Reverse;

            Assert.True(dictionary.ContainsKey(0));
            Assert.False(dictionary.ContainsKey(2));
        }

        [Fact]
        public void ReverseEnumerateTest()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.Dictionary.Reverse;

            var sb = new StringBuilder();
            sb.AppendJoin(",", dictionary);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void ReverseGetTests()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.Dictionary.Reverse;

            Assert.Equal("0", dictionary[0]);
            Assert.True(dictionary.TryGetValue(0, out var value0));
            Assert.Equal("0", value0);

            Assert.False(dictionary.TryGetValue(2, out var value1));
            Assert.Null(value1);

            var ex = Assert.Throws<KeyNotFoundException>(() => dictionary[2]);
            Assert.Equal("The given key '2' was not present in the dictionary.", ex.Message);
        }

        [Fact]
        public void ReverseRemoveTest()
        {
            var testData = new ReverseTestData(3);
            var dictionary = testData.Dictionary.Reverse;

            var wasRemoved0 = dictionary.Remove(0);
            var wasNotRemoved0 = dictionary.Remove(0);

            var wasRemoved1 = dictionary.Remove(1, out var removedValue);
            var wasNotRemoved1 = dictionary.Remove(1, out var notRemovedValue);

            Assert.True(wasRemoved0);
            Assert.False(wasNotRemoved0);
            Assert.True(wasRemoved1);
            Assert.False(wasNotRemoved1);
            Assert.Equal("1", removedValue);
            Assert.Null(notRemovedValue);
            Assert.Equal("[2, 2]-[2, 2]", testData.ExtractResult());
        }

        [Fact]
        public void ReverseSetTest()
        {
            var testData = new ReverseTestData();
            var dictionary = testData.Dictionary.Reverse;

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[2, 1]-[0, 0],[1, 2]", testData.ExtractResult());
        }
    }
}
