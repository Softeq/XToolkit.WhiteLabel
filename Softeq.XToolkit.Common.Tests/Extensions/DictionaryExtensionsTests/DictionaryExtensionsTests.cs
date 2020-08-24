// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.DictionaryExtensionsTests
{
    public class DictionaryExtensionsTests
    {
        private const string Key = "test_key";
        private const string Key2 = "test_key2";

        [Fact]
        public void AddOrReplace_NullDictionary_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                DictionaryExtensions.AddOrReplace(null!, Key, 1);
            });
        }

        [Fact]
        public void AddOrReplace_NewValueInEmptyDictionary_Adds()
        {
            var dictionary = new Dictionary<string, int>();

            dictionary.AddOrReplace(Key, 1);

            Assert.Single(dictionary);
            Assert.Equal(1, dictionary[Key]);
        }

        [Fact]
        public void AddOrReplace_NewValueInNonEmptyDictionary_Adds()
        {
            var dictionary = new Dictionary<string, int> { { Key, 1 } };

            dictionary.AddOrReplace(Key2, 2);

            Assert.Equal(2, dictionary.Count);
            Assert.Equal(1, dictionary[Key]);
            Assert.Equal(2, dictionary[Key2]);
        }

        [Fact]
        public void AddOrReplace_ExistingValueInNonEmptyDictionary_Replaces()
        {
            var dictionary = new Dictionary<string, int> { { Key, 1 } };

            dictionary.AddOrReplace(Key, 2);

            Assert.Single(dictionary);
            Assert.Equal(2, dictionary[Key]);
        }
    }
}
