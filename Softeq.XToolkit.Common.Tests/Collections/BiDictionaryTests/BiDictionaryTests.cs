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
        public void Add()
        {
            var dictionary = BiDictionaryHelper.CreateEmpty();

            dictionary.Add(0, "0");
            var added = dictionary.TryAdd(1, "1");

            Assert.True(added);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", dictionary.GetResult());
        }

        [Fact]
        public void Add_Negative()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            var notAdded = dictionary.TryAdd(1, "1");

            Assert.False(notAdded);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", dictionary.GetResult());
        }

        [Fact]
        public void Add_ThrowsArgumentException()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void Clear()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void Contains()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            Assert.True(dictionary.ContainsKey(0));
        }

        [Fact]
        public void Contains_Negative()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            Assert.False(dictionary.ContainsKey(2));
        }

        [Fact]
        public void Enumerate()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            var sb = new StringBuilder();
            sb.AppendJoin(",", dictionary);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void Get()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            Assert.Equal("0", dictionary[0]);
            Assert.True(dictionary.TryGetValue(0, out var value));
            Assert.Equal("0", value);
        }

        [Fact]
        public void Get_Negative()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            Assert.False(dictionary.TryGetValue(2, out var value));
            Assert.Null(value);
        }

        [Fact]
        public void Get_ThrowsKeyNotFoundException()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            var ex = Assert.Throws<KeyNotFoundException>(() => dictionary[2]);
            Assert.Equal("The given key '2' was not present in the dictionary.", ex.Message);
        }

        [Fact]
        public void Remove()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            var wasRemoved0 = dictionary.Remove(0);
            var wasRemoved1 = dictionary.Remove(1, out var removedValue);

            Assert.True(wasRemoved0);
            Assert.True(wasRemoved1);
            Assert.Equal("1", removedValue);
            Assert.Equal("-", dictionary.GetResult());
        }

        [Fact]
        public void Remove_Negative()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

            var wasNotRemoved0 = dictionary.Remove(2);
            var wasNotRemoved1 = dictionary.Remove(2, out var notRemovedValue);

            Assert.False(wasNotRemoved0);
            Assert.False(wasNotRemoved1);
            Assert.Null(notRemovedValue);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", dictionary.GetResult());
        }

        [Fact]
        public void Serialization()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItems();

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
        public void Set()
        {
            var dictionary = BiDictionaryHelper.CreateEmpty();

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[1, 2]-[0, 0],[2, 1]", dictionary.GetResult());
        }

        [Fact]
        public void Add_Reverse()
        {
            var original = BiDictionaryHelper.CreateEmptyReverse();
            var dictionary = original.Reverse;

            dictionary.Add(0, "0");
            var added = dictionary.TryAdd(1, "1");

            Assert.True(added);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", original.GetResult());
        }

        [Fact]
        public void Add_Negative_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse;

            var notAdded = dictionary.TryAdd(1, "1");

            Assert.False(notAdded);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", original.GetResult());
        }

        [Fact]
        public void Add_ThrowsArgumentException_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
        }

        [Fact]
        public void Clear_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
        }

        [Fact]
        public void Contains_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            Assert.True(dictionary.ContainsKey(0));
        }

        [Fact]
        public void Contains_Negative_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            Assert.False(dictionary.ContainsKey(2));
        }

        [Fact]
        public void Enumerate_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            var sb = new StringBuilder();
            sb.AppendJoin(",", dictionary);

            Assert.Equal("[0, 0],[1, 1]", sb.ToString());
        }

        [Fact]
        public void Get_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            Assert.Equal("0", dictionary[0]);
            Assert.True(dictionary.TryGetValue(0, out var value));
            Assert.Equal("0", value);
        }

        [Fact]
        public void Get_Negative_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            Assert.False(dictionary.TryGetValue(2, out var value));
            Assert.Null(value);
        }

        [Fact]
        public void Get_ThrowsKeyNotFoundException_Reverse()
        {
            var dictionary = BiDictionaryHelper.CreateWithTwoItemsReverse().Reverse;

            var ex = Assert.Throws<KeyNotFoundException>(() => dictionary[2]);
            Assert.Equal("The given key '2' was not present in the dictionary.", ex.Message);
        }

        [Fact]
        public void Remove_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse;

            var wasRemoved0 = dictionary.Remove(0);
            var wasRemoved1 = dictionary.Remove(1, out var removedValue);

            Assert.True(wasRemoved0);
            Assert.True(wasRemoved1);
            Assert.Equal("1", removedValue);
            Assert.Equal("-", original.GetResult());
        }

        [Fact]
        public void Remove_Negative_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse;

            var wasNotRemoved0 = dictionary.Remove(2);
            var wasNotRemoved1 = dictionary.Remove(2, out var notRemovedValue);

            Assert.False(wasNotRemoved0);
            Assert.False(wasNotRemoved1);
            Assert.Null(notRemovedValue);
            Assert.Equal("[0, 0],[1, 1]-[0, 0],[1, 1]", original.GetResult());
        }

        [Fact]
        public void Set_Reverse()
        {
            var original = BiDictionaryHelper.CreateWithTwoItemsReverse();
            var dictionary = original.Reverse;

            dictionary[0] = "0";
            dictionary[1] = "2";

            Assert.Equal("[0, 0],[2, 1]-[0, 0],[1, 2]", original.GetResult());
        }
    }
}
