// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionaryTests
{
    /// <summary>
    ///     Tests for IDictionary interface.
    /// </summary>
    public class DictionaryInterfaceTests
    {
        [Fact]
        public void Add()
        {
            var dictionary = BiDictionaryHelper.CreateEmpty().ToIDictionary();

            dictionary.Add(0, "1");

            Assert.Equal("[0, 1]-[1, 0]", dictionary.GetResult<int, string>());
        }

        [Fact]
        public void Add_ThrowsArgumentException()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void Clear()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void Contains()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            Assert.True(dictionary.Contains(0));
        }

        [Fact]
        public void Contains_Negative()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            Assert.False(dictionary.Contains(2));
        }

        [Fact]
        public void Enumerate()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary().ToIEnumerable();

            var sb = new StringBuilder();
            foreach (KeyValuePair<int, string> item in dictionary)
            {
                sb.Append(item);
            }

            Assert.Equal("[0, 0][1, 1]", sb.ToString());
        }

        [Fact]
        public void Get_ByKey()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            Assert.Equal("0", dictionary[0]);
        }

        [Fact]
        public void Get_ByKey_ReturnsNull()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            Assert.Null(dictionary[2]);
            Assert.Null(dictionary["2"]);
        }

        [Fact]
        public void Remove()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            dictionary.Remove(0);

            Assert.Equal("[1, 1]-[1, 1]", dictionary.GetResult<int, string>());
        }

        [Fact]
        public void Remove_Negative()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            dictionary.Remove(2);

            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", dictionary.GetResult<int, string>());
        }

        [Fact]
        public void Set()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems().ToIDictionary();

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[1, 2]-[0, 0],[1, 1],[2, 1]", dictionary.GetResult<int, string>());
        }

        [Fact]
        public void Add_Reverse()
        {
            var original = BiDictionaryHelper.CreateEmptyReverse();
            var dictionary = original.Reverse.ToIDictionary();

            dictionary.Add(0, "1");

            Assert.Equal("[1, 0]-[0, 1]", original.GetResult());
        }

        [Fact]
        public void Add_ThrowsArgumentException_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary();

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void Clear_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary();

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void Contains_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary();

            Assert.True(dictionary.Contains(0));
        }

        [Fact]
        public void Contains_Negative_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary();

            Assert.False(dictionary.Contains(2));
        }

        [Fact]
        public void Enumerate_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary().ToIEnumerable();

            var sb = new StringBuilder();
            foreach (KeyValuePair<int, string> item in dictionary)
            {
                sb.Append(item);
            }

            Assert.Equal("[0, 0][1, 1]", sb.ToString());
        }

        [Fact]
        public void Get_ByKey_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary();

            Assert.Equal("0", dictionary[0]);
        }

        [Fact]
        public void Get_ByKey_ReturnsNull_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse.ToIDictionary();

            Assert.Null(dictionary[2]);
            Assert.Null(dictionary["2"]);
        }

        [Fact]
        public void Remove_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse.ToIDictionary();

            dictionary.Remove(0);

            Assert.Equal("[1, 1]-[1, 1]", original.GetResult());
        }

        [Fact]
        public void Remove_Negative_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse.ToIDictionary();

            dictionary.Remove(2);

            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", original.GetResult());
        }

        [Fact]
        public void Set_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse.ToIDictionary();

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[1, 1],[2, 1]-[0, 0],[1, 2]", original.GetResult());
        }
    }
}
