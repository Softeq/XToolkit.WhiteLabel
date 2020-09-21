// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public static class ObservableKeyGroupsCollectionHelper
    {
        public static string GroupKeyNull = "Zero";
        public static string GroupKeyEmpty = string.Empty;
        public static string GroupKeyFirst = "First";
        public static string GroupKeySecond = "Second";
        public static string GroupKeyThird = "Third";

        public static IList<string> KeysNull;
        public static IList<string> KeysEmpty = new List<string>();
        public static IList<string> KeysOneNull = new List<string> { null };
        public static IList<string> KeysOneEmpty = new List<string> { string.Empty };
        public static IList<string> KeysOneFill = new List<string> { GroupKeyFirst };
        public static IList<string> KeysTwoFill = new List<string> { GroupKeyFirst, GroupKeySecond };
        public static IList<string> KeysOneFillOneNull = new List<string> { GroupKeyFirst, null };
        public static IList<string> KeysOneFillOneEmpty = new List<string> { GroupKeyFirst, GroupKeyEmpty };
        public static IList<string> KeysDuplicate = new List<string> { GroupKeyFirst, GroupKeyFirst };
        public static IList<string> KeysNotContained = new List<string> { GroupKeyThird };
        public static IList<string> KeysOneContainedOneNotContained = new List<string> { GroupKeyFirst, GroupKeyThird };

        public static IList<int> ItemsNull;
        public static IList<int> ItemsEmpty = new List<int>();
        public static IList<int> ItemsFirst = new List<int> { 1, 2, 3, 4 };
        public static IList<int> ItemsSecond = new List<int> { 4, 5, 6, 7, 8 };
        public static IList<int> ItemsThird = new List<int> { 6, 7, 8 };

        public static IList<KeyValuePair<string, IList<int>>> PairsNull;
        public static IList<KeyValuePair<string, IList<int>>> PairsEmpty = new List<KeyValuePair<string, IList<int>>>();
        public static IList<KeyValuePair<string, IList<int>>> PairsWithKeysWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyFirst, ItemsFirst),
            new KeyValuePair<string, IList<int>>(GroupKeySecond, ItemsSecond)
        };

        public static IList<KeyValuePair<string, IList<int>>> PairsWithKeysWithItemsWithEmpty = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyFirst, ItemsFirst),
            new KeyValuePair<string, IList<int>>(GroupKeyEmpty, ItemsEmpty)
        };

        public static IList<KeyValuePair<string, IList<int>>> PairWithKeyWithNullItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyNull, ItemsNull),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairWithKeyWithEmptyItem = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyEmpty, ItemsEmpty),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairDuplicateKeyWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsThird),
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsThird),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairDuplicateKeyWithNullItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsThird),
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsNull),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairDuplicateKeyWithEmptyItem = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsThird),
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsEmpty),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNullKeyWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(null, ItemsFirst),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNullKeyWithNullItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(null, null),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNullKeyWithEmptyItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(null, new List<int>()),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNotContainedKeyWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsThird),
        };

        public static ObservableKeyGroupsCollection<string, int> CreateWithEmptyGroups()
        {
            return new ObservableKeyGroupsCollection<string, int>(false);
        }

        public static ObservableKeyGroupsCollection<string, int> CreateWithoutEmptyGroups()
        {
            return new ObservableKeyGroupsCollection<string, int>(true);
        }

        public static ObservableKeyGroupsCollection<string, int> CreateFilledGroupsWithEmpty()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(false);
            collection.AddGroups(CreateGroupsWithEmpty());
            return collection;
        }

        public static ObservableKeyGroupsCollection<string, int> CreateFilledGroupsWithoutEmpty()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(true);
            collection.AddGroups(CreateGroupWithoutEmpty());
            return collection;
        }

        public static CollectionEventsCatcher<string, int> CreateCollectionEventCatcher(ObservableKeyGroupsCollection<string, int> collection)
        {
            return CollectionEventsCatcher<string, int>.Create(collection);
        }

        public static ItemsEventsCatcher<string, int> CreateItemsEventCatcher(ObservableKeyGroupsCollection<string, int> collection)
        {
            return ItemsEventsCatcher<string, int>.Create(collection);
        }

        private static IList<KeyValuePair<string, IList<int>>> CreateGroupsWithEmpty()
        {
            return new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, ItemsFirst ),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, ItemsSecond ),
                 new KeyValuePair<string, IList<int>>(GroupKeyEmpty, ItemsEmpty ),
            };
        }

        private static IList<KeyValuePair<string, IList<int>>> CreateGroupWithoutEmpty()
        {
            return new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, ItemsFirst ),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, ItemsSecond ),
            };
        }

        // -- Selectors Object
        public static List<TestItem<string, int>> CreateEmptyItemsList()
        {
            return new List<TestItem<string, int>>();
        }

        public static List<TestItem<string, int>> CreateNullItemsList()
        {
            return null;
        }

        public static List<TestItem<string, int>> CreateFillItemsListWithNull()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyFirst, 13, 1),
                null,
                new TestItem<string, int>(GroupKeySecond, 12, 1)
            };
        }

        public static List<TestItem<string, int>> CreateFillItemsListWithNewKeys()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyFirst, 11, 1),
                new TestItem<string, int>(GroupKeySecond, 12, 1),
                new TestItem<string, int>(GroupKeyFirst, 13, 1),
                new TestItem<string, int>(GroupKeySecond, 14, 1),
                new TestItem<string, int>(GroupKeyThird, 15, 0),
            };
        }

        public static List<TestItem<string, int>> CreateFillItemsListWithExistKeys()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyFirst, 11, 1),
                new TestItem<string, int>(GroupKeySecond, 12, 1),
                new TestItem<string, int>(GroupKeyFirst, 13, 0),
                new TestItem<string, int>(GroupKeySecond, 14, 0)
            };
        }
    }
}
