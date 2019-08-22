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
        [Fact]
        public void AddGroupsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests(groupCollection.AddGroups, listOfFiredActions);

            Assert.True(listOfFiredActions[0].Action == NotifyCollectionChangedAction.Add);

            Assert.True(listOfFiredActions[0].NewItemRanges[0].Index == 1);

            Assert.True(listOfFiredActions[0].OldItemRanges == null);

            Assert.True(listOfFiredActions[0].GroupEvents[0].GroupIndex == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[1].GroupIndex == 2);
            Assert.True(listOfFiredActions[0].GroupEvents[2].GroupIndex == 3);

            Assert.Equal(groupCollection.Count(), 4);

            Assert.Equal(groupCollection.ElementAt(0).Key, "newKey0");
        }

        [Fact]
        public void InsertGroupsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests((groups) => groupCollection.InsertGroups(0, groups), listOfFiredActions);

            Assert.True(listOfFiredActions[0].Action == NotifyCollectionChangedAction.Add);

            Assert.True(listOfFiredActions[0].NewItemRanges[0].Index == 0);

            Assert.True(listOfFiredActions[0].OldItemRanges == null);

            Assert.True(listOfFiredActions[0].GroupEvents[0].GroupIndex == 0);
            Assert.True(listOfFiredActions[0].GroupEvents[1].GroupIndex == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[2].GroupIndex == 2);

            Assert.Equal(groupCollection.Count(), 4);

            Assert.Equal(groupCollection.ElementAt(0).Key, "newKey1");
        }

        [Fact]
        public void RemoveGroupsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, ICollection<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, ICollection<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, ICollection<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.RemoveGroups(new Collection<string> { "newKey3", "newKey0", "newKey1" });

            Assert.Equal(listOfFiredActions.Count, 1);

            Assert.True(listOfFiredActions[0].Action == NotifyCollectionChangedAction.Remove);

            Assert.True(listOfFiredActions[0].NewItemRanges == null);

            Assert.True(listOfFiredActions[0].OldItemRanges != null);

            Assert.Equal(listOfFiredActions[0].OldItemRanges.Count, 2);

            Assert.Equal(listOfFiredActions[0].OldItemRanges[0].Index, 0);
            Assert.Equal(listOfFiredActions[0].OldItemRanges[1].Index, 3);

            Assert.Equal(listOfFiredActions[0].OldItemRanges[0].OldItems.Count, 2);
            Assert.Equal(listOfFiredActions[0].OldItemRanges[1].OldItems.Count, 1);

            Assert.Equal(listOfFiredActions[0].OldItemRanges[0].OldItems[0], "newKey0");
            Assert.Equal(listOfFiredActions[0].OldItemRanges[0].OldItems[1], "newKey1");

            Assert.Equal(listOfFiredActions[0].OldItemRanges[1].OldItems[0], "newKey3");
        }

        [Fact]
        public void ClearAllTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, ICollection<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, ICollection<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, ICollection<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.ClearGroups();

            Assert.Equal(listOfFiredActions.Count, 1);

            Assert.True(listOfFiredActions[0].Action == NotifyCollectionChangedAction.Reset);

            Assert.True(listOfFiredActions[0].NewItemRanges == null);
        }

        [Fact]
        public void ClearGroupTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, ICollection<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, ICollection<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, ICollection<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.ClearGroup("newKey1");

            Assert.Equal(listOfFiredActions.Count, 1);

            Assert.True(listOfFiredActions[0].Action == null);

            Assert.True(listOfFiredActions[0].NewItemRanges == null);

            Assert.True(listOfFiredActions[0].OldItemRanges == null);

            Assert.True(listOfFiredActions[0].GroupEvents != null);

            Assert.Equal(listOfFiredActions[0].GroupEvents.Count, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].GroupIndex, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.Action, NotifyCollectionChangedAction.Reset);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges, null);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges, null);
        }

        [Fact]
        public void AddItemsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, ICollection<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2"}),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.AddItems(new Collection<(string, string)>
                {
                    ("newKey0", "newKey0value0"),
                    ("newKey1", "newKey1value3"),
                    ("newKey1", "newKey1value4")
                }, (item) => item.Item1, (item) => item.Item2);

            Assert.True(listOfFiredActions.Count == 1);

            Assert.Equal(listOfFiredActions[0].Action, null);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(listOfFiredActions[0].GroupEvents.Count, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].Index, 0);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].Index, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].GroupIndex, 0);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].GroupIndex, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.Action, NotifyCollectionChangedAction.Add);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.Action, NotifyCollectionChangedAction.Add);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges.Count, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems.Count, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0], "newKey0value0");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0], "newKey1value3");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[1], "newKey1value4");

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges);
        }

        [Fact]
        public void RemoveItemsTest()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>();
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, ICollection<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, ICollection<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, ICollection<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3", "newKey3value4" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.RemoveItems(new Collection<(string, string)>
                {
                    ("newKey1", "newKey1value1"),
                    ("newKey3", "newKey3value1"),
                    ("newKey3", "newKey3value3"),
                    ("newKey3", "newKey3value4")
                },
                (item) => item.Item1,
                (item) => item.Item2);

            Assert.True(listOfFiredActions.Count == 1);

            Assert.Equal(listOfFiredActions[0].Action, null);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(listOfFiredActions[0].GroupEvents.Count, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].GroupIndex, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].GroupIndex, 3);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.Action, NotifyCollectionChangedAction.Remove);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.Action, NotifyCollectionChangedAction.Remove);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges.Count, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].Index, 0);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].Index, 0);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].Index, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].OldItems.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems.Count, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems[0], "newKey1value1");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].OldItems[0], "newKey3value1");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems[0], "newKey3value3");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems[1], "newKey3value4");
        }

        private void RunAddGroupsTests(Action<Collection<KeyValuePair<string, ICollection<string>>>> action,
            IList<NotifyKeyGroupCollectionChangedEventArgs<string, string>> listOfFiredActions)
        {
            action.Invoke(new Collection<KeyValuePair<string, ICollection<string>>>
            {
                new KeyValuePair<string, ICollection<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, ICollection<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, ICollection<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });

            Assert.True(listOfFiredActions.Count == 1);

            Assert.True(listOfFiredActions[0].NewItemRanges != null);

            Assert.True(listOfFiredActions[0].NewItemRanges.Count == 1);

            Assert.True(listOfFiredActions[0].NewItemRanges[0].NewItems.Count == 3);

            Assert.True(listOfFiredActions[0].NewItemRanges[0].NewItems[0] == "newKey1");
            Assert.True(listOfFiredActions[0].NewItemRanges[0].NewItems[1] == "newKey2");
            Assert.True(listOfFiredActions[0].NewItemRanges[0].NewItems[2] == "newKey3");

            Assert.True(listOfFiredActions[0].GroupEvents != null);

            Assert.True(listOfFiredActions[0].GroupEvents.Count == 3);

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.Action == NotifyCollectionChangedAction.Add));

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.NewItemRanges.Count == 1));

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.NewItemRanges[0].Index == 0));

            Assert.True(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems.Count == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems.Count == 2);
            Assert.True(listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems.Count == 3);

            Assert.True(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0] == "newKey1value1");

            Assert.True(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0] == "newKey2value1");
            Assert.True(listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[1] == "newKey2value2");

            Assert.True(listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems[0] == "newKey3value1");
            Assert.True(listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems[1] == "newKey3value2");
            Assert.True(listOfFiredActions[0].GroupEvents[2].Arg.NewItemRanges[0].NewItems[2] == "newKey3value3");

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Arg.OldItemRanges == null));
        }
    }
}
