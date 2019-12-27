// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionaryTests
{
    /// <summary>
    ///     Tests for IDictionary interface
    /// </summary>
    public class DictionaryInterfaceTests
    {
        [Fact]
        public void AddTest()
        {
            var testData = new TestData();
            var dictionary = testData.DictionaryInterface;

            dictionary.Add(0, "1");

            Assert.Equal("[0, 1]-[1, 0]", testData.ExtractResult());
        }

        [Fact]
        public void AddWithExceptionsTest()
        {
            var testData = new TestData(3);
            var dictionary = testData.DictionaryInterface;

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));
            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void ClearTest()
        {
            var testData = new TestData(3);
            var dictionaryInterface = testData.DictionaryInterface;

            dictionaryInterface.Clear();

            var dictionary = testData.Dictionary;
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void ContainsTest()
        {
            var testData = new TestData(2);
            var dictionary = testData.DictionaryInterface;

            Assert.True(dictionary.Contains(0));
            Assert.False(dictionary.Contains(2));
        }

        [Fact]
        public void EnumerateTest()
        {
            var testData = new TestData(2);
            var dictionary = (IEnumerable) testData.DictionaryInterface;

            var sb = new StringBuilder();
            foreach (KeyValuePair<int, string> item in dictionary)
            {
                sb.Append(item);
            }

            Assert.Equal("[0, 0][1, 1]", sb.ToString());
        }

        [Fact]
        public void GetTests()
        {
            var testData = new TestData(2);
            var dictionary = testData.DictionaryInterface;

            Assert.Equal("0", dictionary[0]);
            Assert.Null(dictionary[2]);
            Assert.Null(dictionary["2"]);
        }

        [Fact]
        public void RemoveTest()
        {
            var testData = new TestData(2);
            var dictionary = testData.DictionaryInterface;

            dictionary.Remove(0);
            dictionary.Remove(0);

            Assert.Equal("[1, 1]-[1, 1]", testData.ExtractResult());
        }

        [Fact]
        public void SetTest()
        {
            var testData = new TestData();
            var dictionary = testData.DictionaryInterface;

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[1, 2]-[0, 0],[2, 1]", testData.ExtractResult());
        }

        //reverse

        [Fact]
        public void ReverseAddTest()
        {
            var testData = new ReverseTestData();
            var dictionary = testData.ReverseDictionaryInterface;

            dictionary.Add(0, "1");

            Assert.Equal("[1, 0]-[0, 1]", testData.ExtractResult());
        }

        [Fact]
        public void ReverseAddWithExceptionsTest()
        {
            var testData = new ReverseTestData(3);
            var dictionary = testData.ReverseDictionaryInterface;

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));
            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void ReverseClearTest()
        {
            var testData = new ReverseTestData(3);
            var dictionaryInterface = testData.ReverseDictionaryInterface;

            dictionaryInterface.Clear();

            var dictionary = testData.Dictionary;
            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void ReverseContainsTest()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.ReverseDictionaryInterface;

            Assert.True(dictionary.Contains(0));
            Assert.False(dictionary.Contains(2));
        }

        [Fact]
        public void ReverseEnumerateTest()
        {
            var testData = new ReverseTestData(2);
            var dictionary = (IEnumerable) testData.ReverseDictionaryInterface;

            var sb = new StringBuilder();
            foreach (KeyValuePair<int, string> item in dictionary)
            {
                sb.Append(item);
            }

            Assert.Equal("[0, 0][1, 1]", sb.ToString());
        }

        [Fact]
        public void ReverseGetTests()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.ReverseDictionaryInterface;

            Assert.Equal("0", dictionary[0]);
            Assert.Null(dictionary[2]);
            Assert.Null(dictionary["2"]);
        }

        [Fact]
        public void ReverseRemoveTest()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.ReverseDictionaryInterface;

            dictionary.Remove(0);
            dictionary.Remove(0);

            Assert.Equal("[1, 1]-[1, 1]", testData.ExtractResult());
        }

        [Fact]
        public void ReverseSetTest()
        {
            var testData = new ReverseTestData(2);
            var dictionary = testData.ReverseDictionaryInterface;

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[1, 1],[2, 1]-[0, 0],[1, 2]", testData.ExtractResult());
        }
    }
}
