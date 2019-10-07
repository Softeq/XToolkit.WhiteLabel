// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionNew
{
    public class ObservableKeyGroupsCollectionNewTests
    {
        #region WITHOUT_EMPTY_GROUPS

        [Fact]
        public void AddGroupsTestWithoutEmpty()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(true);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "key0value0" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests(groupCollection.AddGroups, listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(3, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Equal(4, groupCollection.Count());

            Assert.Equal("newKey0", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void InsertGroupsTestWithoutEmpty()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(true);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "key0value0" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests((groups) => groupCollection.InsertGroups(0, groups), listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.Equal(0, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(3, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Equal(4, groupCollection.Count());

            Assert.Equal("newKey1", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void ReplaceGroupsTestWithoutEmpty()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(true);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "key0value0" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests(groupCollection.ReplaceGroups, listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Replace, listOfFiredActions[0].Action);

            Assert.Equal(0, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(3, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.NotNull(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Equal(3, groupCollection.Count());

            Assert.Equal("newKey1", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void ClearAllTestWithoutEmpty()
        {
            ClearAllTest(true);
        }

        [Fact]
        public void ClearGroupTestWithoutEmpty()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(true);

            var listOfFiredActions = RunClearGroupTest(groupCollection);

            Assert.Equal(NotifyCollectionChangedAction.Remove, listOfFiredActions[0].Action);

            Assert.NotNull(listOfFiredActions[0].OldItemRanges);

            Assert.Equal(1, listOfFiredActions[0].OldItemRanges.Count);

            Assert.Equal(1, listOfFiredActions[0].OldItemRanges[0].Index);

            Assert.Equal(1, listOfFiredActions[0].OldItemRanges[0].OldItems.Count);

            Assert.Equal("newKey1", listOfFiredActions[0].OldItemRanges[0].OldItems[0]);

            Assert.Null(listOfFiredActions[0].GroupEvents);
        }

        [Fact]
        public void AddItemsTestWithoutEmpty()
        {
            AddItemsTest(true);
        }

        [Fact]
        public void AddItemsTest2WithoutEmpty()
        {
            AddItemsTest2(true);
        }

        [Fact]
        public void InsertItemsTestWithoutEmpty()
        {
            InsertItemsTest(true);
        }

        [Fact]
        public void ReplaceItemsTestWithoutEmpty()
        {
            ReplaceItemsTest(true);
        }

        [Fact]
        public void RemoveItemsTestWithoutEmptyGroups()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(true);

            var listOfFiredActions = RemoveItemsTest(groupCollection);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(3, listOfFiredActions[0].GroupEvents[0].GroupIndex);

            Assert.Equal(NotifyCollectionChangedAction.Remove, listOfFiredActions[0].GroupEvents[0].Arg.Action);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);

            Assert.Equal(2, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges.Count);

            Assert.Equal(0, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].Index);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[1].Index);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[1].OldItems.Count);

            Assert.Equal("newKey3value1", listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems[0]);
            Assert.Equal("newKey3value3", listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[1].OldItems[0]);
            Assert.Equal("newKey3value4", listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[1].OldItems[1]);

            Assert.Equal(NotifyCollectionChangedAction.Remove, listOfFiredActions[0].Action);

            Assert.NotNull(listOfFiredActions[0].OldItemRanges);

            Assert.Equal(1, listOfFiredActions[0].OldItemRanges.Count);

            Assert.Equal(1, listOfFiredActions[0].OldItemRanges[0].Index);

            Assert.Equal(1, listOfFiredActions[0].OldItemRanges[0].OldItems.Count);

            Assert.Equal("newKey1", listOfFiredActions[0].OldItemRanges[0].OldItems[0]);

            Assert.Equal(3, groupCollection.Count());
            Assert.Single(groupCollection.ElementAt(2));
        }

        #endregion

        #region ALLOW_EMPTY_GROUPS

        [Fact]
        public void AddGroupsTestAllowEmptyGroups()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests(groupCollection.AddGroups, listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(4, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey4", listOfFiredActions[0].NewItemRanges[0].NewItems[3]);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Equal(5, groupCollection.Count());

            Assert.Equal("newKey0", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void InsertGroupsTestAllowEmptyGroups()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests((groups) => groupCollection.InsertGroups(0, groups), listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.Equal(0, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(4, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey4", listOfFiredActions[0].NewItemRanges[0].NewItems[3]);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Equal(5, groupCollection.Count());

            Assert.Equal("newKey1", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void RemoveGroupsTestAllowEmptyGroups()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

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
        public void ReplaceGroupsTestAllowEmptyGroups()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            RunAddGroupsTests(groupCollection.ReplaceGroups, listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Replace, listOfFiredActions[0].Action);

            Assert.Equal(0, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(4, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey4", listOfFiredActions[0].NewItemRanges[0].NewItems[3]);

            Assert.NotNull(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Equal(4, groupCollection.Count());

            Assert.Equal("newKey1", groupCollection.ElementAt(0).Key);
        }

        [Fact]
        public void RemoveItemsTestAllowEmptyGroups()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);

            var listOfFiredActions = RemoveItemsTest(groupCollection);

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

            Assert.Equal(2, listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems.Count);
            Assert.Equal(1, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].OldItems.Count);
            Assert.Equal(2, listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems.Count);

            Assert.Equal("newKey1value1", listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems[0]);
            Assert.Equal("newKey1value2", listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges[0].OldItems[1]);
            Assert.Equal("newKey3value1", listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[0].OldItems[0]);
            Assert.Equal("newKey3value3", listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems[0]);
            Assert.Equal("newKey3value4", listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges[1].OldItems[1]);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.Empty(groupCollection.ElementAt(1));
            Assert.Single(groupCollection.ElementAt(3));
        }

        [Fact]
        public void ClearAllTestAllowEmpty()
        {
            ClearAllTest(false);
        }

        [Fact]
        public void ClearGroupTestAllowEmpty()
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(false);

            var listOfFiredActions = RunClearGroupTest(groupCollection);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].GroupIndex);

            Assert.Equal(NotifyCollectionChangedAction.Reset, listOfFiredActions[0].GroupEvents[0].Arg.Action);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges);

            Assert.Empty(groupCollection.ElementAt(1));
        }

        [Fact]
        public void AddItemsTestAllowEmpty()
        {
            AddItemsTest(false);
        }

        [Fact]
        public void AddItemsTest2AllowEmpty()
        {
            AddItemsTest2(false);
        }

        [Fact]
        public void InsertItemsTestAllowEmpty()
        {
            InsertItemsTest(false);
        }

        [Fact]
        public void ReplaceItemsTestAllowEmpty()
        {
            ReplaceItemsTest(false);
        }

        #endregion

        private void ClearAllTest(bool withoutEmptyGroups)
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(withoutEmptyGroups);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.ClearGroups();

            Assert.Single(listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Reset, listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Empty(groupCollection);
        }

        public List<NotifyKeyGroupCollectionChangedEventArgs<string, string>> RunClearGroupTest(
            ObservableKeyGroupsCollectionNew<string, string> groupCollection)
        {
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "newKey0value0" }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.ClearGroup("newKey1");

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            return listOfFiredActions;
        }

        private void AddItemsTest(bool withoutEmptyGroups)
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(withoutEmptyGroups);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "newKey0value0" }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2"}),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.AddItems(new Collection<(string, string)>
                {
                    ("newKey0", "newKey0value1"),
                    ("newKey1", "newKey1value3"),
                    ("newKey1", "newKey1value4")
                }, (item) => item.Item1, (item) => item.Item2);

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].Action);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(2, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].Index);
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

            Assert.Equal("newKey0value1", listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value3", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value4", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[1]);

            Assert.Null(listOfFiredActions[0].GroupEvents[0].Arg.OldItemRanges);
            Assert.Null(listOfFiredActions[0].GroupEvents[1].Arg.OldItemRanges);

            Assert.Equal(2, groupCollection.ElementAt(0).Count());
        }

        private void AddItemsTest2(bool withoutEmptyGroups)
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(withoutEmptyGroups);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "newKey0value0" }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2"}),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.AddItems(new Collection<(string, string)>
                {
                    ("newKey0", "newKey0value0"),
                    ("newKey1", "newKey1value3"),
                    ("newKey1", "newKey1value4"),
                    ("newKey2", "newKey2value0")
                }, (item) => item.Item1, (item) => item.Item2);

            Assert.Single(listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Add, listOfFiredActions[0].Action);

            Assert.NotNull(listOfFiredActions[0].NewItemRanges);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges.Count);

            Assert.Equal(2, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey2", listOfFiredActions[0].NewItemRanges[0].NewItems[0]);

            Assert.Null(listOfFiredActions[0].OldItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            Assert.Equal(2, listOfFiredActions[0].GroupEvents.Count);

            Assert.Equal(1, listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].Index);
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

            Assert.Equal(2, groupCollection.ElementAt(0).Count());
        }

        private void InsertItemsTest(bool withoutEmptyGroups)
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(withoutEmptyGroups);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "newKey0value0" }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value4"}),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.InsertItems(new Collection<(string, string, int)>
                {
                    ("newKey0", "newKey0value1", 0),
                    ("newKey1", "newKey1value0", 0),
                    ("newKey1", "newKey1value2", 2),
                    ("newKey1", "newKey1value3", 3),
                    ("newKey1", "newKey1value5", 5)
                },
                (item) => item.Item1,
                (item) => item.Item2,
                (item) => item.Item3);

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

            Assert.Equal("newKey0value1", listOfFiredActions[0].GroupEvents[0].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value0", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1value2", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[1].NewItems[0]);
            Assert.Equal("newKey1value3", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[1].NewItems[1]);
            Assert.Equal("newKey1value5", listOfFiredActions[0].GroupEvents[1].Arg.NewItemRanges[2].NewItems[0]);
        }

        private void ReplaceItemsTest(bool withoutEmptyGroups)
        {
            var groupCollection = new ObservableKeyGroupsCollectionNew<string, string>(withoutEmptyGroups);
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "oldKey0value0" }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2"}),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.ReplaceItems(new Collection<(string, string)>
                {
                    ("newKey0", "newKey0value0"),
                    ("newKey1", "newKey1value3"),
                    ("newKey1", "newKey1value4")
                }, (item) => item.Item1, (item) => item.Item2);

            Assert.Single(listOfFiredActions);

            Assert.Equal(NotifyCollectionChangedAction.Replace, listOfFiredActions[0].Action);

            Assert.NotNull(listOfFiredActions[0].NewItemRanges);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges.Count);

            Assert.Equal(0, listOfFiredActions[0].NewItemRanges[0].Index);

            Assert.Equal(2, listOfFiredActions[0].NewItemRanges[0].NewItems.Count);

            Assert.Equal("newKey0", listOfFiredActions[0].NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey1", listOfFiredActions[0].NewItemRanges[0].NewItems[1]);

            Assert.NotNull(listOfFiredActions[0].OldItemRanges);

            Assert.Null(listOfFiredActions[0].GroupEvents);

            Assert.Single(groupCollection.ElementAt(0));
            Assert.Equal(2, groupCollection.ElementAt(1).Count());
        }

        public List<NotifyKeyGroupCollectionChangedEventArgs<string, string>> RemoveItemsTest(ObservableKeyGroupsCollectionNew<string, string> groupCollection)
        {
            var listOfFiredActions = new List<NotifyKeyGroupCollectionChangedEventArgs<string, string>>();

            groupCollection.AddGroups(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey0", new Collection<string> { "newKey0value0" }),
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1", "newKey1value2" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3", "newKey3value4" }),
            });
            groupCollection.ItemsChanged += (sender, args) =>
            {
                listOfFiredActions.Add(args);
            };

            groupCollection.RemoveItems(new Collection<(string, string)>
                {
                    ("newKey1", "newKey1value1"),
                    ("newKey1", "newKey1value2"),
                    ("newKey3", "newKey3value1"),
                    ("newKey3", "newKey3value3"),
                    ("newKey3", "newKey3value4")
                },
                (item) => item.Item1,
                (item) => item.Item2);

            Assert.Single(listOfFiredActions);

            Assert.Null(listOfFiredActions[0].NewItemRanges);

            Assert.NotNull(listOfFiredActions[0].GroupEvents);

            return listOfFiredActions;
        }

        private void RunAddGroupsTests(Action<Collection<KeyValuePair<string, IList<string>>>> action,
            IList<NotifyKeyGroupCollectionChangedEventArgs<string, string>> listOfFiredActions)
        {
            action.Invoke(new Collection<KeyValuePair<string, IList<string>>>
            {
                new KeyValuePair<string, IList<string>>("newKey1", new Collection<string> { "newKey1value1" }),
                new KeyValuePair<string, IList<string>>("newKey2", new Collection<string> { "newKey2value1", "newKey2value2"}),
                new KeyValuePair<string, IList<string>>("newKey3", new Collection<string> { "newKey3value1", "newKey3value2", "newKey3value3" }),
                new KeyValuePair<string, IList<string>>("newKey4", new Collection<string> { }),
            });

            Assert.Equal(1, listOfFiredActions.Count);

            Assert.NotNull(listOfFiredActions[0].NewItemRanges);

            Assert.Equal(1, listOfFiredActions[0].NewItemRanges.Count);

            Assert.Equal("newKey1", listOfFiredActions[0].NewItemRanges[0].NewItems[0]);
            Assert.Equal("newKey2", listOfFiredActions[0].NewItemRanges[0].NewItems[1]);
            Assert.Equal("newKey3", listOfFiredActions[0].NewItemRanges[0].NewItems[2]);

            Assert.Null(listOfFiredActions[0].GroupEvents);
        }
    }
}
