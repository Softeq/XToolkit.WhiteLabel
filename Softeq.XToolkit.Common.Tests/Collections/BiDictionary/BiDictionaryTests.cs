// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Softeq.XToolkit.Common.Collections;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.BiDictionary
{
    public class BiDictionaryTests
    {
        [Fact]
        public void AddTest()
        {
            var dictionary = new BiDictionary<int, string>();
            var dictionaryInterface = (IDictionary) dictionary;
            var reverseDictionaryInterface = (IDictionary) dictionary.Reverse;
            var collectionInterface = (ICollection<KeyValuePair<int, string>>) dictionary;
            var reverseCollectionInterface = (ICollection<KeyValuePair<string, int>>) dictionary.Reverse;

            dictionary.Add(0, "0");
            var added = dictionary.TryAdd(1, "1");
            var notAdded = dictionary.TryAdd(1, "1");
            collectionInterface.Add(new KeyValuePair<int, string>(2, "2"));
            dictionaryInterface.Add(3, "3");

            dictionary.Reverse.Add("4", 4);
            var addedReverse = dictionary.Reverse.TryAdd("5", 5);
            var notAddedReverse = dictionary.Reverse.TryAdd("5", 5);
            reverseCollectionInterface.Add(new KeyValuePair<string, int>("6", 6));
            reverseDictionaryInterface.Add("7", 7);

            var keysResult = string.Join(",", dictionary.Keys);
            var valuesResult = string.Join(",", dictionary.Values);
            var reverseKeysResult = string.Join(",", dictionary.Reverse.Keys);
            var reverseValuesResult = string.Join(",", dictionary.Reverse.Values);

            const string result = "0,1,2,3,4,5,6,7";

            Assert.True(added);
            Assert.False(notAdded);
            Assert.True(addedReverse);
            Assert.False(notAddedReverse);
            Assert.Equal(result, keysResult);
            Assert.Equal(result, valuesResult);
            Assert.Equal(result, reverseKeysResult);
            Assert.Equal(result, reverseValuesResult);
        }

        [Fact]
        public void AddWithExceptionsTest()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"},
                {2, "2"}
            };

            var dictionaryInterface = (IDictionary) dictionary;
            var reverseDictionaryInterface = (IDictionary) dictionary.Reverse;
            var collectionInterface = (ICollection<KeyValuePair<int, string>>) dictionary;
            var reverseCollectionInterface = (ICollection<KeyValuePair<string, int>>) dictionary.Reverse;

            var ex = Assert.Throws<ArgumentException>(() => dictionary.Add(1, "1"));
            var exCollection =
                Assert.Throws<ArgumentException>(() => collectionInterface.Add(new KeyValuePair<int, string>(1, "1")));
            var exDictionary = Assert.Throws<ArgumentException>(() => dictionaryInterface.Add(1, "1"));

            var exReverse = Assert.Throws<ArgumentException>(() => dictionary.Reverse.Add("1", 1));
            var exCollectionReverse = Assert.Throws<ArgumentException>(() =>
                reverseCollectionInterface.Add(new KeyValuePair<string, int>("1", 1)));
            var exDictionaryReverse = Assert.Throws<ArgumentException>(() => reverseDictionaryInterface.Add("1", 1));

            Assert.Equal("An item with the same key has already been added. Key: 1", ex.Message);
            Assert.Equal("An item with the same key has already been added. Key: 1", exCollection.Message);
            Assert.Equal("An item with the same key has already been added. Key: 1", exDictionary.Message);
            Assert.Equal("An item with the same key has already been added. Key: 1", exReverse.Message);
            Assert.Equal("An item with the same key has already been added. Key: 1", exCollectionReverse.Message);
            Assert.Equal("An item with the same key has already been added. Key: 1", exDictionaryReverse.Message);
        }

        [Fact]
        public void ClearTest()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"}
            };

            dictionary.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Reverse);
            Assert.Empty(dictionary.Reverse.Values);
            Assert.Empty(dictionary.Reverse.Keys);

            dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"}
            };

            dictionary.Reverse.Clear();

            Assert.Empty(dictionary);
            Assert.Empty(dictionary.Values);
            Assert.Empty(dictionary.Keys);
            Assert.Empty(dictionary.Reverse);
            Assert.Empty(dictionary.Reverse.Values);
            Assert.Empty(dictionary.Reverse.Keys);
        }

        [Fact]
        public void ContainsTest()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"}
            };

            Assert.True(dictionary.ContainsKey(0));
            Assert.False(dictionary.ContainsKey(2));
            Assert.True(dictionary.Reverse.ContainsKey("0"));
            Assert.False(dictionary.Reverse.ContainsKey("2"));

            Assert.Contains(dictionary.FirstOrDefault(), dictionary);
            Assert.DoesNotContain(new KeyValuePair<int, string>(2, "2"), dictionary);
            Assert.Contains(dictionary.Reverse.FirstOrDefault(), dictionary.Reverse);
            Assert.DoesNotContain(new KeyValuePair<string, int>("2", 2), dictionary.Reverse);

            var dictionaryInterface = (IDictionary) dictionary;
            var reverseDictionaryInterface = (IDictionary) dictionary.Reverse;

            Assert.True(dictionaryInterface.Contains(0));
            Assert.False(dictionaryInterface.Contains(2));
            Assert.True(reverseDictionaryInterface.Contains("0"));
            Assert.False(reverseDictionaryInterface.Contains("2"));
        }

        [Fact]
        public void EnumerateTest()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"}
            };

            var sb = new StringBuilder();

            sb.AppendJoin(",", dictionary);

            var enumerable = (IEnumerable) dictionary;

            sb.AppendJoin(",", enumerable.Cast<KeyValuePair<int, string>>());

            var dictionaryInterface = (IDictionary) dictionary;

            sb.AppendJoin(",", dictionaryInterface.Cast<KeyValuePair<int, string>>());

            //reverse

            sb.AppendJoin(",", dictionary.Reverse);

            var enumerableReverse = (IEnumerable) dictionary.Reverse;

            sb.AppendJoin(",", enumerableReverse.Cast<KeyValuePair<string, int>>());

            var dictionaryInterfaceReverse = (IDictionary) dictionary.Reverse;

            sb.AppendJoin(",", dictionaryInterfaceReverse.Cast<KeyValuePair<string, int>>());

            Assert.Equal("[0, 0],[1, 1][0, 0],[1, 1][0, 0],[1, 1][0, 0],[1, 1][0, 0],[1, 1][0, 0],[1, 1]",
                sb.ToString());
        }

        [Fact]
        public void GetTests()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"}
            };

            Assert.Equal("0", dictionary[0]);
            Assert.Equal(0, dictionary.Reverse["0"]);

            Assert.True(dictionary.TryGetValue(0, out var value0));
            Assert.Equal("0", value0);
            Assert.True(dictionary.Reverse.TryGetValue("0", out var value1));
            Assert.Equal(0, value1);

            Assert.False(dictionary.TryGetValue(2, out var value2));
            Assert.Null(value2);
            Assert.False(dictionary.Reverse.TryGetValue("2", out var value3));
            Assert.Equal(0, value3);

            var ex = Assert.Throws<KeyNotFoundException>(() => dictionary[2]);
            var exReverse = Assert.Throws<KeyNotFoundException>(() => dictionary.Reverse["2"]);

            Assert.Equal("The given key '2' was not present in the dictionary.", ex.Message);
            Assert.Equal("The given key '2' was not present in the dictionary.", exReverse.Message);

            var dictionaryInterface = (IDictionary) dictionary;
            var reverseDictionaryInterface = (IDictionary) dictionary.Reverse;

            Assert.Equal("0", dictionaryInterface[0]);
            Assert.Equal(0, reverseDictionaryInterface["0"]);

            Assert.Null(dictionaryInterface[2]);
            Assert.Null(dictionaryInterface["2"]);
        }

        [Fact]
        public void RemoveTest()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"},
                {2, "2"},
                {3, "3"},
                {4, "4"},
                {5, "5"},
                {6, "6"},
                {7, "7"}
            };

            var dictionaryInterface = (IDictionary) dictionary;
            var reverseDictionaryInterface = (IDictionary) dictionary.Reverse;
            var collectionInterface = (ICollection<KeyValuePair<int, string>>) dictionary;
            var reverseCollectionInterface = (ICollection<KeyValuePair<string, int>>) dictionary.Reverse;

            var item = collectionInterface.ElementAt(3);
            var reverseItem = reverseCollectionInterface.ElementAt(7);

            var wasRemoved0 = dictionary.Remove(0);
            var wasNotRemoved0 = dictionary.Remove(0);

            var wasRemoved1 = dictionary.Remove(1, out var removedValue);
            var wasNotRemoved1 = dictionary.Remove(1, out var notRemovedValue);

            dictionaryInterface.Remove(2);
            dictionaryInterface.Remove(2);

            var wasRemovedFromCollection = collectionInterface.Remove(item);
            var wasNotRemovedFromCollection = collectionInterface.Remove(item);

            //reverse

            var wasRemovedReverse0 = dictionary.Reverse.Remove("4");
            var wasNotRemovedReverse0 = dictionary.Reverse.Remove("4");

            var wasRemovedReverse1 = dictionary.Reverse.Remove("5", out var removedValueReverse);
            var wasNotRemovedReverse1 = dictionary.Reverse.Remove("5", out var notRemovedValueReverse);

            reverseDictionaryInterface.Remove("6");
            reverseDictionaryInterface.Remove("6");

            var wasRemovedFromCollectionReverse = reverseCollectionInterface.Remove(reverseItem);
            var wasNotRemovedFromCollectionReverse = reverseCollectionInterface.Remove(reverseItem);

            Assert.True(wasRemoved0);
            Assert.False(wasNotRemoved0);
            Assert.True(wasRemoved1);
            Assert.False(wasNotRemoved1);
            Assert.Equal("1", removedValue);
            Assert.Null(notRemovedValue);
            Assert.True(wasRemovedFromCollection);
            Assert.False(wasNotRemovedFromCollection);

            Assert.True(wasRemovedReverse0);
            Assert.False(wasNotRemovedReverse0);
            Assert.True(wasRemovedReverse1);
            Assert.False(wasNotRemovedReverse1);
            Assert.Equal(5, removedValueReverse);
            Assert.Equal(0, notRemovedValueReverse);
            Assert.True(wasRemovedFromCollectionReverse);
            Assert.False(wasNotRemovedFromCollectionReverse);

            var keysResult = string.Join(",", dictionary.Keys);
            var valuesResult = string.Join(",", dictionary.Values);
            var reverseKeysResult = string.Join(",", dictionary.Reverse.Keys);
            var reverseValuesResult = string.Join(",", dictionary.Reverse.Values);

            Assert.Equal("7", keysResult);
            Assert.Equal("7", valuesResult);
            Assert.Equal("3", reverseKeysResult);
            Assert.Equal("3", reverseValuesResult);
        }

        [Fact]
        public void SerializationTest()
        {
            var dictionary = new BiDictionary<int, string>
            {
                {0, "0"},
                {1, "1"}
            };

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
            var dictionary = new BiDictionary<int, string>();
            dictionary[0] = "0";
            dictionary.Reverse["1"] = 1;
            dictionary[1] = "2";
            dictionary.Reverse["2"] = 2;

            var keysResult = string.Join(",", dictionary.Keys);
            var valuesResult = string.Join(",", dictionary.Values);
            var reverseKeysResult = string.Join(",", dictionary.Reverse.Keys);
            var reverseValuesResult = string.Join(",", dictionary.Reverse.Values);

            Assert.Equal("0,2", keysResult);
            Assert.Equal("0,2", valuesResult);
            Assert.Equal("0,2", reverseKeysResult);
            Assert.Equal("0,2", reverseValuesResult);
        }
    }
}