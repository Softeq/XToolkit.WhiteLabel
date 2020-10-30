// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTests
    {
        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void Clear_EmptyCollection_CollectionEmpty(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.Clear();

            Assert.Equal(0, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void Clear_FilledCollection_CollectionCleared(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.Clear();

            Assert.Equal(0, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void GetEnumerator_AddCorrectPairs_AllElementsPassed(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            collection.AddGroups(pairs);

            var isSame = true;

            var keyEnumerator = collection.GetEnumerator();
            var isNextKey = keyEnumerator.MoveNext();

            for (int i = 0; isNextKey && isSame; i++)
            {
                var element = keyEnumerator.Current;
                isNextKey = keyEnumerator.MoveNext();

                isSame = element.Key == pairs[i].Key;

                var valEnumerator = element.GetEnumerator();
                var isNextValue = valEnumerator.MoveNext();

                for (int j = 0; isNextValue && isSame; j++)
                {
                    isSame = valEnumerator.Current == pairs[i].Value[j];
                    isNextValue = valEnumerator.MoveNext();
                }
            }

            Assert.True(isSame);
        }
    }
}
