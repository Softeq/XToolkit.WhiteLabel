// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTestsRemoveGroups
    {
        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_NullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveGroups(CollectionHelper.KeysNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_ContainsKey_CollectionSizeDecreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var keys = CollectionHelper.KeysOneFill;
            var expectedCount = collection.Count - keys.Count;

            collection.RemoveGroups(keys);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_MissingKey_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(
                () => collection.RemoveGroups(CollectionHelper.KeysNotContained));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveGroups_ContainsAndMissKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(
                () => collection.RemoveGroups(CollectionHelper.KeysOneContainedOneNotContained));
        }
    }
}
