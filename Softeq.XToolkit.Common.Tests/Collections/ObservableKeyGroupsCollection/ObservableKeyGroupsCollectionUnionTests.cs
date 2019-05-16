// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xunit;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Tests.Core.Common.Collections.ObservableKeyGroupsCollection.Helpers;
using static Softeq.XToolkit.Tests.Core.Common.Collections.ObservableKeyGroupsCollection.Helpers.ObservableKeyGroupsCollectionHelpers;

namespace Softeq.XToolkit.Tests.Core.Common.Collections.ObservableKeyGroupsCollection
{
    public class ObservableKeyGroupsCollectionUnionTests : IDisposable
    {
        private ObservableKeyGroupsCollection<DateTimeOffset, TestItemModel> _groupCollection1;
        private ObservableKeyGroupsCollection<DateTimeOffset, TestItemModel> _groupCollection2;

        public ObservableKeyGroupsCollectionUnionTests()
        {
            var list1 = CreateRandList(4, 5, "Old");
            _groupCollection1 = CreateSortedGroupCollection(list1);

            var list2 = CreateRandList(17, 7, "New");
            _groupCollection2 = CreateSortedGroupCollection(list2);
        }

        public void Dispose()
        {
            _groupCollection1 = null;
            _groupCollection2 = null;
        }

        [Fact]
        public void UnionItemsTest()
        {
            // arrange
            var items1 = _groupCollection1.Values.ToList();
            var items2 = _groupCollection2.Values.ToList();

            // act
            _groupCollection1.UnionSortedGroups(_groupCollection2, new MessageComparer());

            // assert
            var mergedItems = _groupCollection1.Values.ToList();

            Assert.True(mergedItems.Intersect(items1).Count() == items1.Count);
            Assert.True(mergedItems.Intersect(items2).Count() == items2.Count);
        }

        [Fact]
        public void UnionResultKeysTest()
        {
            // arrange
            var groupCollection1Count = _groupCollection1.Count;
            var groupsKeys1 = _groupCollection1.Keys.ToList();

            var groupCollection2Count = _groupCollection2.Count;
            var groupsKeys2 = _groupCollection2.Keys.ToList();

            // act
            _groupCollection1.UnionSortedGroups(_groupCollection2, new MessageComparer());

            // assert
            var mergedGroupCollectionCount = _groupCollection1.Count;
            var mergedGroupsKeys = _groupCollection1.Keys.ToList();

            // intersect
            Assert.True(mergedGroupsKeys.Intersect(groupsKeys1).Count() == groupsKeys1.Count);
            Assert.True(mergedGroupsKeys.Intersect(groupsKeys2).Count() == groupsKeys2.Count);
            // size
            Assert.Equal(Math.Max(mergedGroupCollectionCount, Math.Max(groupCollection1Count, groupCollection2Count)),
                mergedGroupCollectionCount);
            // order
            Assert.True(groupsKeys1.First() == mergedGroupsKeys.First() || groupsKeys2.First() == mergedGroupsKeys.First());
            Assert.True(groupsKeys1.Last() == mergedGroupsKeys.Last() || groupsKeys2.Last() == mergedGroupsKeys.Last());
        }

        [Fact]
        public void UnionCollectionChangedActionTest()
        {
            // arrange
            var listOfFiredActions = new List<NotifyCollectionChangedAction>();

            _groupCollection1.ItemsChanged += (sender, e) =>
            {
                listOfFiredActions.Add(e.Action);
            };

            // act
            _groupCollection1.UnionSortedGroups(_groupCollection2, new MessageComparer());

            // assert
            var resetEvents = listOfFiredActions.Count(x => x == NotifyCollectionChangedAction.Reset);

            Assert.Equal(1, resetEvents);
        }
    }
}
