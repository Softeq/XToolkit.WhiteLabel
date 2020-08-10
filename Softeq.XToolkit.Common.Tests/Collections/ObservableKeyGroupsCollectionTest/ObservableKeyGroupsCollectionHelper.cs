using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public static class ObservableKeyGroupsCollectionHelper
    {
        public static string KeysParameterName = "keys";
        public static string ItemsParameterName = "items";

        public static string GroupKeyEmpty = string.Empty;
        public static string GroupKeyFirst = "First";
        public static string GroupKeySecond = "Second";

        public static IList<string> KeysNull;
        public static IList<string> KeysEmpty = new List<string>();
        public static IList<string> KeysOneNull = new List<string> { null };
        public static IList<string> KeysOneEmpty = new List<string> { string.Empty };
        public static IList<string> KeysOneFill = new List<string> { GroupKeyFirst };
        public static IList<string> KeysTwoFill = new List<string> { GroupKeyFirst, GroupKeySecond };
        public static IList<string> KeysOneFillOneEmpty = new List<string> { GroupKeyFirst, GroupKeyEmpty };

        public static IList<int> ItemsNull;
        public static IList<int> ItemsEmpty = new List<int>();
        public static IList<int> ItemsFirst = new List<int> { 1, 2, 3 };
        public static IList<int> ItemsSecond = new List<int> { 4, 5, 6, 7, 8 };

        public static IList<KeyValuePair<string, IList<int>>> PairsNull;
        public static IList<KeyValuePair<string, IList<int>>> PairsEmpty = new List<KeyValuePair<string, IList<int>>>();
        public static IList<KeyValuePair<string, IList<int>>> PairsWithKeysWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> ( GroupKeyFirst, ItemsFirst),
            new KeyValuePair<string, IList<int>> ( GroupKeySecond, ItemsSecond)
        };

        public static IList<KeyValuePair<string, IList<int>>> PairsWithKeysWithItemsWithEmpty = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> ( GroupKeyFirst, ItemsFirst),
            new KeyValuePair<string, IList<int>> ( GroupKeyEmpty, ItemsEmpty)
        };

        public static IList<KeyValuePair<string, IList<int>>> PairWithKeyWithNullItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsNull),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairWithKeyWithEmptyItem = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsEmpty),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairDuplicateKeyWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsFirst),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairDuplicateKeyWithNullItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsNull),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairDuplicateKeyWithEmptyItem = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsEmpty),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNullKeyWithItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (null, ItemsFirst),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNullKeyWithNullItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (null, null),
        };

        public static IList<KeyValuePair<string, IList<int>>> PairNullKeyWithEmptyItems = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>> (null, new List<int>()),
        };

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

        public static CollectionEventsCatcher<string, int> CreateCollectionEventCatcher(ObservableKeyGroupsCollection<string, int> collection)
        {
            return CollectionEventsCatcher<string, int>.Create(collection);
        }

        public static ItemsEventsCatcher<string, int> CreateItemsEventCatcher(ObservableKeyGroupsCollection<string, int> collection)
        {
            return ItemsEventsCatcher<string, int>.Create(collection);
        }

        private static IList<KeyValuePair<string, IList<int>>> CreateTwoItemGroupWithEmpty()
        {
            return new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsFirst ),
                 new KeyValuePair<string, IList<int>> (GroupKeyEmpty, ItemsEmpty ),
            };
        }

        private static IList<KeyValuePair<string, IList<int>>> CreateTwoItemGroupWithoutEmpty()
        {
            return new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>> (GroupKeyFirst, ItemsFirst ),
                 new KeyValuePair<string, IList<int>> (GroupKeySecond, ItemsSecond ),
            };
        }
    }
}