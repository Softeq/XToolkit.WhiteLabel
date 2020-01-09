using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public static class ObservableKeyGroupsCollectionHelper
    {
        public static string EmptyGroupKey = string.Empty;
        public static string FirstGroupKey = "First";
        public static string SecondKeyGroup = "Second";

        public static IList<string> NullKeys;
        public static IList<string> EmptyKeys = new List<string>();
        public static IList<string> OneGroupKeys = new List<string> { FirstGroupKey };
        public static IList<string> TwoGroupKeys = new List<string> { FirstGroupKey, SecondKeyGroup };
        public static IList<string> TwoWithEmptyGroupKeys = new List<string> { FirstGroupKey, EmptyGroupKey };

        public static IList<int> FirstGroupList = new List<int> { 1, 2, 3 };
        public static IList<int> SecondGroupList = new List<int> { 4, 5, 6, 7, 8 };
        public static IList<int> SecondEmptyGroupList = new List<int>();

        public static ObservableKeyGroupsCollection<string, int> CreateEmptyWithEmptyGroups()
        {
            return new ObservableKeyGroupsCollection<string, int>(false);
        }

        public static ObservableKeyGroupsCollection<string, int> CreateEmptyWithoutEmptyGroups()
        {
            return new ObservableKeyGroupsCollection<string, int>(true);
        }

        public static ObservableKeyGroupsCollection<string, int> CreateWithItemsWithEmptyGroups()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(false);
            collection.AddGroups(CreateTwoItemGroupWithEmpty());

            return collection;
        }

        public static ObservableKeyGroupsCollection<string, int> CreateWithItemsWithoutEmptyGroups()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(true);
            collection.AddGroups(CreateTwoItemGroupWithoutEmpty());

            return collection;
        }

        public static CollectionEventsCatcher<string, int> CreateEventCatcher(ObservableKeyGroupsCollection<string, int> collection)
        {
            return CollectionEventsCatcher<string, int>.Create(collection);
        }
        private static IList<KeyValuePair<string, IList<int>>> CreateTwoItemGroupWithEmpty()
        {
            return new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>> (FirstGroupKey, FirstGroupList ),
                 new KeyValuePair<string, IList<int>> (SecondKeyGroup, SecondEmptyGroupList ),
            };
        }

        private static IList<KeyValuePair<string, IList<int>>> CreateTwoItemGroupWithoutEmpty()
        {
            return new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>> (FirstGroupKey, FirstGroupList ),

            };
        }
    }
}