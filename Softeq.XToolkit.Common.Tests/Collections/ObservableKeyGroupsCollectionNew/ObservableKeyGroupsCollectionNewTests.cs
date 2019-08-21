using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
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

            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Value == 1);

            Assert.True(listOfFiredActions[0].OldItems == null);

            Assert.True(listOfFiredActions[0].GroupEvents[0].Key == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[1].Key == 2);
            Assert.True(listOfFiredActions[0].GroupEvents[2].Key == 3);

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

            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Value == 0);

            Assert.True(listOfFiredActions[0].OldItems == null);

            Assert.True(listOfFiredActions[0].GroupEvents[0].Key == 0);
            Assert.True(listOfFiredActions[0].GroupEvents[1].Key == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[2].Key == 2);

            Assert.Equal(groupCollection.Count(), 4);

            Assert.Equal(groupCollection.ElementAt(0).Key, "newKey1");
        }

        [Fact]
        public void ReplaceGroupsTest()
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

            RunAddGroupsTests(groupCollection.ReplaceGroups, listOfFiredActions);

            //TODO Add oldItems check

            Assert.True(listOfFiredActions[0].Action == NotifyCollectionChangedAction.Replace);

            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Value == 0);

            Assert.True(listOfFiredActions[0].GroupEvents[0].Key == 0);
            Assert.True(listOfFiredActions[0].GroupEvents[1].Key == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[2].Key == 2);

            Assert.Equal(groupCollection.Count(), 3);

            Assert.Equal(groupCollection.ElementAt(0).Key, "newKey1");
        }

        [Fact]
        public void RemoveGroupsTest ()
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

            groupCollection.RemoveGroups(new Collection<string> { "newKey3", "newKey0", "newKey1"});

            Assert.Equal(listOfFiredActions.Count, 1);

            Assert.True(listOfFiredActions[0].NewItems == null);

            Assert.True(listOfFiredActions[0].OldItems != null);

            Assert.Equal(listOfFiredActions[0].OldItems.Count, 2);

            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(0).Value, 0);
            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(1).Value, 3);

            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(0).Key.Count, 2);
            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(1).Key.Count, 1);

            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(0).Key[0], "newKey0");
            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(0).Key[1], "newKey1");

            Assert.Equal(listOfFiredActions[0].OldItems.ElementAt(1).Key[0], "newKey3");
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

            Assert.True(listOfFiredActions[0].NewItems == null);
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

            Assert.True(listOfFiredActions[0].NewItems == null);

            Assert.True(listOfFiredActions[0].OldItems == null);

            Assert.True(listOfFiredActions[0].GroupEvents != null);

            Assert.Equal(listOfFiredActions[0].GroupEvents.Count, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Key, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.Action, NotifyCollectionChangedAction.Reset);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.OldItems, null);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.NewItems, null);
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

            groupCollection.AddItems(
                new Collection<(string, string)>
                {
                    ("newKey0", "newKey0value0"),
                    ("newKey1", "newKey1value3"),
                    ("newKey1", "newKey1value4"),
                    ("newKey2", "newKey2value0")
                },
                (item) => item.Item1,
                (item) => item.Item2);

            Assert.True(listOfFiredActions.Count == 1);

            Assert.Equal(listOfFiredActions[0].Action, NotifyCollectionChangedAction.Add);

            Assert.NotNull(listOfFiredActions[0].NewItems);

            Assert.Equal(listOfFiredActions[0].NewItems.Count, 1);

            Assert.Equal(listOfFiredActions[0].NewItems.ElementAt(0).Key.Count, 1);

            Assert.Equal(listOfFiredActions[0].NewItems.ElementAt(0).Key.ElementAt(0), "newKey2");

            Assert.Equal(listOfFiredActions[0].NewItems.ElementAt(0).Value, 2);

            Assert.Equal(listOfFiredActions[0].OldItems, null);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(listOfFiredActions[0].GroupEvents.Count, 3);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Key, 0);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Key, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[2].Key, 2);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.Action, NotifyCollectionChangedAction.Add);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Value.Action, NotifyCollectionChangedAction.Add);
            Assert.Equal(listOfFiredActions[0].GroupEvents[2].Value.Action, NotifyCollectionChangedAction.Add);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Value.NewItems);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[1].Value.NewItems);
            Assert.NotNull(listOfFiredActions[0].GroupEvents[2].Value.NewItems);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.NewItems.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Value.NewItems.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[2].Value.NewItems.Count, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.NewItems.ElementAt(0).Key.Count, 1);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Key.Count, 2);
            Assert.Equal(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Key.Count, 1);

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.NewItems.ElementAt(0).Key[0], "newKey0value0");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Key[0], "newKey1value3");
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Key[1], "newKey1value4");
            Assert.Equal(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Key[0], "newKey2value0");

            Assert.Equal(listOfFiredActions[0].GroupEvents[0].Value.NewItems.ElementAt(0).Value, 0);
            Assert.Equal(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Value, 2);
            Assert.Equal(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Value, 0);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Value.OldItems);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Value.OldItems);
            Assert.Null(listOfFiredActions[0].GroupEvents[2].Value.OldItems);
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

            Assert.True(listOfFiredActions[0].NewItems != null);

            Assert.True(listOfFiredActions[0].NewItems.Count == 1);

            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Key.Count == 3);

            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Key[0] == "newKey1");
            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Key[1] == "newKey2");
            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Key[2] == "newKey3");

            Assert.True(listOfFiredActions[0].GroupEvents != null);

            Assert.True(listOfFiredActions[0].GroupEvents.Count == 3);

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Value.Action == NotifyCollectionChangedAction.Add));

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Value.NewItems.Count == 1));

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Value.NewItems.ElementAt(0).Value == 0));

            Assert.True(listOfFiredActions[0].GroupEvents[0].Value.NewItems.ElementAt(0).Key.Count == 1);
            Assert.True(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Key.Count == 2);
            Assert.True(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Key.Count == 3);

            Assert.True(listOfFiredActions[0].GroupEvents[0].Value.NewItems.ElementAt(0).Key[0] == "newKey1value1");

            Assert.True(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Key[0] == "newKey2value1");
            Assert.True(listOfFiredActions[0].GroupEvents[1].Value.NewItems.ElementAt(0).Key[1] == "newKey2value2");

            Assert.True(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Key[0] == "newKey3value1");
            Assert.True(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Key[1] == "newKey3value2");
            Assert.True(listOfFiredActions[0].GroupEvents[2].Value.NewItems.ElementAt(0).Key[2] == "newKey3value3");

            Assert.True(listOfFiredActions[0].GroupEvents.All(x => x.Value.OldItems == null));
        }
    }
}
