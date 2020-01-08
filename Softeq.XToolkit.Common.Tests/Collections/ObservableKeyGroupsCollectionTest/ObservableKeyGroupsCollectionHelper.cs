using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public static class ObservableKeyGroupsCollectionHelper
    {
        public static string FirstGroupKey = "First";
        public static string SecondKeyGroup = "Second";

        public static IEnumerable<string> NullKeys;
        public static IEnumerable<string> EmptyKeys = new List<string>();

        public static IList<int> FirstGroupList = new List<int> { 1, 2, 3 };
        public static IList<int> SecondGroupList = new List<int> { 4, 5, 6, 7, 8 };

        public static ObservableKeyGroupsCollection<string, int> CreateEmptyWithEmptyGroups()
        {
            return new ObservableKeyGroupsCollection<string, int>(false);
        }

        public static ObservableKeyGroupsCollection<string, int> CreateEmptyWithoutEmptyGroups()
        {
            return new ObservableKeyGroupsCollection<string, int>(true);
        }

        //public static ObservableKeyGroupsCollection<string, int> CreateOneGroup()
        //{
        //    return new ObservableKeyGroupsCollection<string, int>();
        //}

        //public static ObservableKeyGroupsCollection<string, int> Create()
        //{
        //    return new ObservableKeyGroupsCollection<string, int>();
        //}
    }
}