// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTestsClearGroup
    {
        [Fact]
        public void ClearGroup_ForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(
                () => collection.ClearGroup(CollectionHelper.GroupKeyFirst));
        }

        [Fact]
        public void ClearGroup_AllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(
                () => collection.ClearGroup(null));
        }

        [Fact]
        public void ClearGroup_AllowEmptyGroupExistKey_ItemsRemoved()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var key = CollectionHelper.GroupKeyFirst;
            var startCount = collection.FirstOrDefault(x => x.Key == key)?.Count() ?? 0;

            collection.ClearGroup(key);

            var currentCount = collection.FirstOrDefault(x => x.Key == key)?.Count() ?? -1;

            Assert.True((startCount > 0) && currentCount == 0);
        }

        [Fact]
        public void ClearGroup_AllowEmptyGroupMissingKey_KeyNotFoundException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<KeyNotFoundException>(
                () => collection.ClearGroup(CollectionHelper.GroupKeyThird));
        }
    }
}
