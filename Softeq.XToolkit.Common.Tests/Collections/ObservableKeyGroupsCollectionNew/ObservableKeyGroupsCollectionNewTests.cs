﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.EventArguments;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionNew
{
    public class ObservableKeyGroupsCollectionNewTests
    {
        [Theory]
        //[InlineData(0, 0, -1, -1)]
        [InlineData(1, 1, 1, 1)]
        [InlineData(5, 5, 1, 1)]
        [InlineData(87, 9, 11, 1)]
        [InlineData(1000, 9, 112, 111)]
        public void SimpleAddGroupsTest(int itemsCount, int groupsCount, int firstGroupSize, int lastGroupSize)
        {
            var rawItems = Enumerable.Range(1, itemsCount);
    
            var groups = rawItems
                .GroupBy(item => item.ToString().Substring(0, 1))
                .Select(group => new KeyValuePair<string, IList<int>>(group.Key, group.ToList()));
    
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, int>(false);
            groupCollection.AddGroups(groups);

            var count = groupCollection.Count();
            
            Assert.Equal(groupsCount, count);
            Assert.Equal(firstGroupSize, count > 0 ? groupCollection.First().Value.Count : -1);
            Assert.Equal(lastGroupSize, count > 0 ? groupCollection.Last().Value.Count : -1);
        }
        
        [Fact]
        public void AddGroupsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>())
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            RunAddGroupsTests(groupCollection.AddGroups, listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].GroupIndex);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].GroupIndex);
            Assert.Equal(3, listOfFiredActions[0].GroupEvents[2].GroupIndex);

            Assert.Equal(4, groupCollection.Count());

            Assert.Equal("newKey0", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void AddItemsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>()),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2" })
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            groupCollection.AddItems(
                new Collection<(string, string)>
                {
                    ("newKey0", "newKey0value0"),
                    ("newKey1", "newKey1value3"),
                    ("newKey1", "newKey1value4")
                }, item => item.Item1, item => item.Item2);

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(2, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].Index);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].Index);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].GroupIndex);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].GroupIndex);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].GroupEvents[0].Arg.Action);
            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].GroupEvents[1].Arg.Action);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges.Count);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges.Count);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey0value0", listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value3", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value4", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[1]);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges);

            Assert.Equal(1, groupCollection.ElementAt(0).Value.Count);
        }

        [Fact]
        public void ClearAllTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>()),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2" }),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" })
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            groupCollection.ClearGroups();

            Assert.Single(listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Reset, listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Empty(groupCollection);
        }

        [Fact]
        public void ClearGroupTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>()),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2" }),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" })
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            groupCollection.ClearGroup("newKey1");

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].GroupIndex);

            Assert.Equal(NotifyCollectionChangedAction.Reset, listOfFiredActions[0].GroupEvents[0].Arg.Action);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);

            Assert.Equal(0, groupCollection.ElementAt(1).Value.Count);
        }

        [Fact]
        public void InsertGroupsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>())
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            RunAddGroupsTests(groups => groupCollection.InsertGroups(0, groups), listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.Equal(0, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].GroupIndex);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].GroupIndex);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[2].GroupIndex);

            Assert.Equal(4, groupCollection.Count());

            Assert.Equal("newKey1", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void InsertItemsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>()),
                new KeyValuePair<string, IList<string>>("newKey1",
                    new Collection<string> { "newKey1value1", "newKey1value4" })
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            groupCollection.InsertItems(
                new Collection<(string, string, int)>
                {
                    ("newKey0", "newKey0value0", 0),
                    ("newKey1", "newKey1value0", 0),
                    ("newKey1", "newKey1value2", 2),
                    ("newKey1", "newKey1value3", 3),
                    ("newKey1", "newKey1value5", 5)
                },
                item => item.Item1,
                item => item.Item2,
                item => item.Item3);

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(2, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].GroupIndex);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].GroupIndex);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].GroupEvents[0].Arg.Action);
            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].GroupEvents[1].Arg.Action);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges.Count);
            Assert.Equal(3, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges.Count);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].Index);
            Assert.Equal(0, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].Index);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[1].Index);
            Assert.Equal(5, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[2].Index);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems.Count);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[1].NewItems.Count);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[2].NewItems.Count);

            Assert.Equal("newKey0value0", listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value0", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value2", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[1].NewItems[0]);
            Assert.Equal("newKey1value3", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[1].NewItems[1]);
            Assert.Equal("newKey1value5", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[2].NewItems[0]);
        }

        [Fact]
        public void RemoveGroupsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>()),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2" }),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" })
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            groupCollection.RemoveGroups(new Collection<string> { "newKey3", "newKey0", "newKey1" });

            Assert.Single(listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Remove, listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.NotNull(listOfFiredActions[0].OldItemRanges);

            Assert.Equal(2, listOfFiredActions[0].OldItemRanges.Count);

            Assert.Equal(0, listOfFiredActions[0].OldItemRanges[0].Index);
            Assert.Equal(3, listOfFiredActions[0].OldItemRanges[1].Index);

            Assert.Equal(2, listOfFiredActions[0].OldItemRanges[0].OldItems.Count);
            Assert.Equal(1, listOfFiredActions[0].OldItemRanges[1].OldItems.Count);

            Assert.Equal("newKey0", listOfFiredActions[0].OldItemRanges[0].OldItems[0]);
            Assert.Equal("newKey1", listOfFiredActions[0].OldItemRanges[0].OldItems[1]);

            Assert.Equal("newKey3", listOfFiredActions[0].OldItemRanges[1].OldItems[0]);

            Assert.Single(groupCollection);
        }

        [Fact]
        public void RemoveItemsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string>()),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2" }),
                new KeyValuePair<string, IList<string>>("newKey3",
                    new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3", "newKey3value4" })
            });
            groupCollection.ItemsChanged += (sender, args) => { listOfFiredActions.Add(args); };

            groupCollection.RemoveItems(
                new Collection<(string, string)>
                {
                    ("newKey1", "newKey1value1"),
                    ("newKey3", "newKey3value1"),
                    ("newKey3", "newKey3value3"),
                    ("newKey3", "newKey3value4")
                },
                item => item.Item1,
                item => item.Item2);

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(2, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].GroupIndex);
            Assert.Equal(3, listOfFiredActions[0].GroupEvents[1].GroupIndex);

            Assert.Equal(NotifyCollectionChangedAction.Remove, listOfFiredActions[0].GroupEvents[0].Arg.Action);
            Assert.Equal(NotifyCollectionChangedAction.Remove, listOfFiredActions[0].GroupEvents[1].Arg.Action);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges.Count);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].Index);
            Assert.Equal(0, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].Index);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].Index);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems.Count);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].OldItems.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems.Count);

            Assert.Equal("newKey1value1", listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems[0]);
            Assert.Equal("newKey3value1", listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].OldItems[0]);
            Assert.Equal("newKey3value3", listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems[0]);
            Assert.Equal("newKey3value4", listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems[1]);

            Assert.Equal(1, groupCollection.ElementAt(1).Value.Count);
            Assert.Equal(1, groupCollection.ElementAt(3).Value.Count);
        }
        
        private void RunAddGroupsTests(Action<Collection<KeyValuePair<string, IList<string>>>> action,
            IList<NotifyKeyGroupCollectionChangedEventArgs<string, string>> listOfFiredActions)
        {
            action.Invoke(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2" }),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" })
            });

            Assert.Equal(1, listOfFiredActions.Count);

            Assert.NotNull(listOfFiredActions[0].NewItemRanges);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges.Count);

            Assert.Equal(3, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey1", listOfFiredActions[0].NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey2", listOfFiredActions[0].NewItemRanges[0].NewItems[1]);
            Assert.Equal("newKey3", listOfFiredActions[0].NewItemRanges[0].NewItems[2]);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(3, listOfFiredActions[0].GroupEvents.Count);

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.Action == NotifyCollectionChangedAction.Add));

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.NewItemRanges.Count == 1));

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.NewItemRanges[0].Index == 0));

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems.Count);
            Assert.Equal(3, listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey1value1", listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0]);

            Assert.Equal("newKey2value1", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey2value2", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[1]);

            Assert.Equal("newKey3value1", listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey3value2", listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems[1]);
            Assert.Equal("newKey3value3", listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems[2]);

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.OldItemRanges == null));
        }
    }
}
