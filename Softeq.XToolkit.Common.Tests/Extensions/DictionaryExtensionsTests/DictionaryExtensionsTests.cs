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
        [Fact]
        public void AddOrReplace_NullDictionary_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() =>
            {
                DictionaryExtensions.AddOrReplace(null!, "test_key", 1);
            });
        }

        [Fact]
        public void AddOrReplace_NewValueInEmptyDictionary_Adds()
        {
            var dictionary = new Dictionary<string, int>();

            dictionary.AddOrReplace("test_key", 1);

            Assert.NotEmpty(dictionary);
        }

        [Fact]
        public void AddOrReplace_NewValueInNonEmptyDictionary_Adds()
        {
            var dictionary = new Dictionary<string, int> { { "test_key1", 1 } };

            dictionary.AddOrReplace("test_key2", 2);

            Assert.Equal(2, dictionary.Count);
        }

        [Fact]
        public void AddOrReplace_ExistValueInNonEmptyDictionary_Replaces()
        {
            const string Key = "test_key";
            var dictionary = new Dictionary<string, int> { { Key, 1 } };

            dictionary.AddOrReplace(Key, 2);

            Assert.Equal(2, dictionary[Key]);
        }
    }
}
