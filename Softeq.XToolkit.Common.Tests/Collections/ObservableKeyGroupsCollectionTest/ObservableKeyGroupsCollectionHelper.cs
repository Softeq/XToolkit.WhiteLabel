// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;
using Xunit;

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
        private static readonly IList<int> ItemsFirst = new List<int> { 1, 2, 3, 4 };
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

        public static IList<KeyValuePair<string, IList<int>>> PairNotContainedKeyWithEmptyItem = new List<KeyValuePair<string, IList<int>>>()
        {
            new KeyValuePair<string, IList<int>>(GroupKeyThird, ItemsEmpty),
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
                null,
                new TestItem<string, int>(GroupKeyFirst, 13, 1),
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
                new TestItem<string, int>(GroupKeyFirst, 11, 2),
                new TestItem<string, int>(GroupKeySecond, 12, 1),
                new TestItem<string, int>(GroupKeyFirst, 13, 2),
                new TestItem<string, int>(GroupKeySecond, 14, 0)
            };
        }

        public static TheoryData<ObservableKeyGroupsCollection<string, int>> EmptyCollectionOptionsTestData
            => new TheoryData<ObservableKeyGroupsCollection<string, int>>
            {
                CreateWithEmptyGroups(),
                CreateWithoutEmptyGroups()
            };

        public static TheoryData<ObservableKeyGroupsCollection<string, int>> FillCollectionOptionsTestData
            => new TheoryData<ObservableKeyGroupsCollection<string, int>>
            {
                CreateFilledGroupsWithEmpty(),
                CreateFilledGroupsWithoutEmpty()
            };

        public static TheoryData<ObservableKeyGroupsCollection<string, int>> CollectionOptionsTestData
            => new TheoryData<ObservableKeyGroupsCollection<string, int>>
            {
                CreateWithEmptyGroups(),
                CreateWithoutEmptyGroups(),
                CreateFilledGroupsWithEmpty(),
                CreateFilledGroupsWithoutEmpty()
            };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            IList<KeyValuePair<string, IList<int>>>> ReplaceGroupsWithItemsNullKeyTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                IList<KeyValuePair<string, IList<int>>>>
            {
                { CreateFilledGroupsWithEmpty(), PairNullKeyWithItems },
                { CreateFilledGroupsWithoutEmpty(), PairNullKeyWithItems },
                { CreateFilledGroupsWithEmpty(), PairNullKeyWithEmptyItems },
                { CreateFilledGroupsWithoutEmpty(), PairNullKeyWithEmptyItems },
                { CreateFilledGroupsWithEmpty(), PairNullKeyWithNullItems },
                { CreateFilledGroupsWithoutEmpty(), PairNullKeyWithNullItems }
            };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            ObservableKeyGroupsCollection<string, int>> InsertItemsListItemsWithExistKeysTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                ObservableKeyGroupsCollection<string, int>>
            {
                { CreateFilledGroupsWithEmpty(), CreateFillItemsListWithExistKeys(), InsertItemsListItemsWithExistKeysWithEmptyResult() },
                { CreateFilledGroupsWithoutEmpty(), CreateFillItemsListWithExistKeys(), InsertItemsListItemsWithExistKeysWithoutEmptyResult() }
            };

        public static TheoryData<ObservableKeyGroupsCollection<string, int>, List<TestItem<string, int>>, int>
            InsertItemsIndexOutOfBoundsTestData
            => new TheoryData<ObservableKeyGroupsCollection<string, int>, List<TestItem<string, int>>, int>
            {
                { CreateFilledGroupsWithEmpty(), InserItemsWithExistKeys(), int.MinValue },
                { CreateFilledGroupsWithoutEmpty(), InserItemsWithExistKeys(), int.MaxValue }
            };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            ObservableKeyGroupsCollection<string, int>> RemoveItemsListItemsWithExistKeysTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                ObservableKeyGroupsCollection<string, int>>
                {
                        { CreateFilledGroupsWithEmpty(), RemovedExistItems(), RemoveItemsExistKeysWithEmptyResult() },
                        { CreateFilledGroupsWithoutEmpty(), RemovedExistItems(), RemoveItemsExistKeysWithoutEmptyResult() }
                };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            ObservableKeyGroupsCollection<string, int>> RemoveItemsAllItemsForExistKeyForbidEmptyTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                ObservableKeyGroupsCollection<string, int>>
                {
                    { CreateFilledGroupsWithoutEmpty(), RemoveItemsAllExistItemsForGroup(), RemoveItemsAllItemsForExistKeyForbidEmptyResult() }
                };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            ObservableKeyGroupsCollection<string, int>> RemoveItemsAllItemsForExistKeyAllowEmptyTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                ObservableKeyGroupsCollection<string, int>>
                {
                    { CreateFilledGroupsWithEmpty(), RemoveItemsAllExistItemsForGroup(), RemoveItemsAllItemsForExistKeyAllowEmptyResult() }
                };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            List<string>> RemoveItemsEventsTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                List<string>>
                {
                    { CreateFilledGroupsWithEmpty(), RemoveItemsAllExistItemsForGroup(), new List<string>() { GroupKeyFirst } },
                    { CreateFilledGroupsWithoutEmpty(), RemoveItemsAllExistItemsForGroup(), new List<string> { GroupKeyFirst } }
                };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            List<string>> CollectionChangedRemoveItemsForbidEmptyGroupRemoveAllGroupItemsTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                List<string>>
                {
                    { CreateFilledGroupsWithoutEmpty(), RemoveItemsAllExistItemsForGroup(), new List<string> { GroupKeyFirst } }
                };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>,
            ObservableKeyGroupsCollection<string, int>> ReplaceItemsListItemsTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>,
                ObservableKeyGroupsCollection<string, int>>
                {
                    { CreateFilledGroupsWithEmpty(), ReplaceItemsItemsList(), ReplaceItemsListItemsResult() },
                    { CreateFilledGroupsWithoutEmpty(), ReplaceItemsItemsList(), ReplaceItemsListItemsResult() }
                };

        public static TheoryData<
            ObservableKeyGroupsCollection<string, int>,
            List<TestItem<string, int>>> ReplaceItemsEventsTestData
            => new TheoryData<
                ObservableKeyGroupsCollection<string, int>,
                List<TestItem<string, int>>>
                {
                    { CreateFilledGroupsWithEmpty(), ReplaceItemsItemsList() },
                    { CreateFilledGroupsWithoutEmpty(), ReplaceItemsItemsList() }
                };

        private static List<TestItem<string, int>> InserItemsWithExistKeys()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyFirst, 11, 2),
                new TestItem<string, int>(GroupKeySecond, 12, 1),
                new TestItem<string, int>(GroupKeyFirst, 13, 2),
                new TestItem<string, int>(GroupKeySecond, 14, 0)
            };
        }

        private static ObservableKeyGroupsCollection<string, int> InsertItemsListItemsWithExistKeysWithEmptyResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(false);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, new List<int> { 1, 2, 11, 13, 3, 4 }),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, new List<int> { 14, 12, 4, 5, 6, 7, 8 }),
                 new KeyValuePair<string, IList<int>>(GroupKeyEmpty, new List<int>()),
            };

            collection.AddGroups(items);

            return collection;
        }

        private static ObservableKeyGroupsCollection<string, int> InsertItemsListItemsWithExistKeysWithoutEmptyResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(true);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, new List<int> { 1, 2, 11, 13, 3, 4 }),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, new List<int> { 14, 12, 4, 5, 6, 7, 8 }),
            };

            collection.AddGroups(items);

            return collection;
        }

        private static List<TestItem<string, int>> RemovedExistItems()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyFirst, 1),
                new TestItem<string, int>(GroupKeySecond, 5),
                new TestItem<string, int>(GroupKeyFirst, 3),
                new TestItem<string, int>(GroupKeySecond, 8)
            };
        }

        private static ObservableKeyGroupsCollection<string, int> RemoveItemsExistKeysWithEmptyResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(false);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, new List<int> { 2, 4 }),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, new List<int> { 4, 6, 7 }),
                 new KeyValuePair<string, IList<int>>(GroupKeyEmpty, new List<int>()),
            };

            collection.AddGroups(items);

            return collection;
        }

        private static ObservableKeyGroupsCollection<string, int> RemoveItemsExistKeysWithoutEmptyResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(true);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, new List<int> { 2, 4 }),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, new List<int> { 4, 6, 7 }),
            };

            collection.AddGroups(items);

            return collection;
        }

        private static List<TestItem<string, int>> RemoveItemsAllExistItemsForGroup()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyFirst, 1),
                new TestItem<string, int>(GroupKeyFirst, 3),
                new TestItem<string, int>(GroupKeySecond, 8),
                new TestItem<string, int>(GroupKeySecond, 5),
                new TestItem<string, int>(GroupKeyFirst, 2),
                new TestItem<string, int>(GroupKeyFirst, 4),
            };
        }

        private static ObservableKeyGroupsCollection<string, int> RemoveItemsAllItemsForExistKeyAllowEmptyResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(false);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, new List<int>()),
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, new List<int> { 4, 6, 7 }),
                 new KeyValuePair<string, IList<int>>(GroupKeyEmpty, new List<int>()),
            };

            collection.AddGroups(items);

            return collection;
        }

        private static ObservableKeyGroupsCollection<string, int> RemoveItemsAllItemsForExistKeyForbidEmptyResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(true);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeySecond, new List<int> { 4, 6, 7 }),
            };

            collection.AddGroups(items);

            return collection;
        }

        private static List<TestItem<string, int>> ReplaceItemsItemsList()
        {
            return new List<TestItem<string, int>>
            {
                new TestItem<string, int>(GroupKeyThird, 1),
                new TestItem<string, int>(GroupKeyThird, 3),
                new TestItem<string, int>(GroupKeyFirst, 4),
            };
        }

        private static ObservableKeyGroupsCollection<string, int> ReplaceItemsListItemsResult()
        {
            var collection = new ObservableKeyGroupsCollection<string, int>(false);
            var items = new List<KeyValuePair<string, IList<int>>>
            {
                 new KeyValuePair<string, IList<int>>(GroupKeyFirst, new List<int>() { 4 }),
                 new KeyValuePair<string, IList<int>>(GroupKeyThird, new List<int>() { 1, 3 }),
            };

            collection.AddGroups(items);

            return collection;
        }
    }
}