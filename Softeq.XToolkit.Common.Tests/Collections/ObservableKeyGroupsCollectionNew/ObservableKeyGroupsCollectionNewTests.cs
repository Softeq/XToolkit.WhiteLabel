﻿using System;
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

            Assert.True(listOfFiredActions[0].NewItems.ElementAt(0).Value == 1);

            Assert.True(listOfFiredActions[0].OldItems == null);

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

            Assert.Equal(groupCollection.Count(), 3);

            Assert.Equal(groupCollection.ElementAt(0).Key, "newKey1");
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

            Assert.True(listOfFiredActions[0].GroupEvents[0].Key == "newKey1");
            Assert.True(listOfFiredActions[0].GroupEvents[1].Key == "newKey2");
            Assert.True(listOfFiredActions[0].GroupEvents[2].Key == "newKey3");

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
