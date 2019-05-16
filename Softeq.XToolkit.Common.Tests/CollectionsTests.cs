// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common
{
    public class CollectionsTests
    {
        private const string CollectionKey = "collection";
        private const string CollectionItemKey = "collection_item";

        [Theory]
        [MemberData(nameof(ObservableKeyGroupsCollectionTestData.DataToAdd), MemberType =
            typeof(ObservableKeyGroupsCollectionTestData))]
        public void AddToObservableKeyGroupsCollectionTest(
            (Func<string, string> DefaultSelector, Func<string, string> CustomSelector, List<List<string>> ValuesToAdd)
                input, string result)
        {
            var collection = new ObservableKeyGroupsCollection<string, string>(input.DefaultSelector);
            foreach (var item in input.ValuesToAdd)
            {
                collection.AddRangeToGroups(item, input.CustomSelector);
            }

            Assert.Equal(CollectionToString(collection), result);
        }

        [Theory]
        [MemberData(nameof(CollectionSorterTestData.SortParams), MemberType =
            typeof(CollectionSorterTestData))]
        public void CollectionSorterTest(bool isAsc, ObservableCollection<string> collection, string result)
        {
            var stringComparision = new Comparison<string>(
                (s1, s2) => string.Compare(s1, s2, StringComparison.CurrentCulture));

            if (isAsc)
            {
                collection.Sort(stringComparision);
            }
            else
            {
                collection.DescendingSort(stringComparision);
            }

            Assert.Equal(collection.Aggregate(string.Empty, (current, item) => current + item), result);
        }

        [Theory]
        [MemberData(nameof(ObservableKeyGroupsCollectionTestData.DataToRemove), MemberType =
            typeof(ObservableKeyGroupsCollectionTestData))]
        public void RemoveFromObservableKeyGroupsCollectionTest(
            (Func<string, string> DefaultSelector,
                Func<string, string> CustomSelector,
                ObservableKeyGroupsCollection<string, string> Collection,
                List<string> Values) input, string result)
        {
            foreach (var item in input.Values)
            {
                input.Collection.RemoveFromGroups(item, input.CustomSelector);
            }

            Assert.Equal(CollectionToString(input.Collection), result);
        }

        private static string CollectionToString(ObservableKeyGroupsCollection<string, string> collection)
        {
            var itemsCount = string.Join(",", collection.Select(x => x.Count));
            var result =
                $"values:{string.Join(",", collection.Values)};keys:{string.Join(",", collection.Keys)};counts:{collection.Count},{itemsCount}";
            return result;
        }

        [Fact]
        public void NotificationsObservableKeyGroupsCollectionTest()
        {
            var listOfFiredActions = new List<Tuple<string, NotifyCollectionChangedAction>>();
            var collection = new ObservableKeyGroupsCollection<string, string>(x => x[0].ToString().ToLower());

            collection.CollectionChanged += (sender, e) =>
            {
                listOfFiredActions.Add(new Tuple<string, NotifyCollectionChangedAction>(CollectionKey, e.Action));
            };

            collection.AddRangeToGroups(new[] {"aa", "ba", "ca"});

            collection[0].CollectionChanged += (sender, e) =>
            {
                listOfFiredActions.Add(
                    new Tuple<string, NotifyCollectionChangedAction>(CollectionItemKey, e.Action));
            };

            collection[1].CollectionChanged += (sender, e) =>
            {
                listOfFiredActions.Add(
                    new Tuple<string, NotifyCollectionChangedAction>(CollectionItemKey, e.Action));
            };

            collection[2].CollectionChanged += (sender, e) =>
            {
                listOfFiredActions.Add(
                    new Tuple<string, NotifyCollectionChangedAction>(CollectionItemKey, e.Action));
            };

            collection.AddRangeToGroups(new[] {"ab", "ac", "ad", "bb", "bc"});
            collection.RemoveFromGroups("ac");
            collection.RemoveFromGroups("bc");
            collection.RemoveFromGroups("ca");
            collection.AddRangeToGroups(new[] {"ca", "bc"});
            collection.ReplaceRangeGroup(new[] {"aa", "ab", "ba"});

            var collectionEvents = listOfFiredActions.Where(x => x.Item1 == CollectionKey).ToList();
            var addEvents = collectionEvents.Count(x => x.Item2 == NotifyCollectionChangedAction.Add);
            var removeEvents = collectionEvents.Count(x => x.Item2 == NotifyCollectionChangedAction.Remove);
            var resetEvents = collectionEvents.Count(x => x.Item2 == NotifyCollectionChangedAction.Reset);

            var itemEvents = listOfFiredActions.Where(x => x.Item1 == CollectionItemKey).ToList();
            var addItemEvents = itemEvents.Count(x => x.Item2 == NotifyCollectionChangedAction.Add);
            var removeItemEvents = itemEvents.Count(x => x.Item2 == NotifyCollectionChangedAction.Remove);

            //assert
            Assert.Equal(2, addEvents);
            Assert.Equal(1, removeEvents);
            Assert.Equal(1, resetEvents);

            Assert.Equal(3, addItemEvents);
            Assert.Equal(3, removeItemEvents);
        }
    }

    internal static class ObservableKeyGroupsCollectionTestData
    {
        private static readonly Func<string, string> GetFirstLatterLower = x => x[0].ToString().ToLower();
        private static readonly Func<string, string> GetFirstLatterUpper = x => x[0].ToString().ToUpper();

        public static IEnumerable<object[]> DataToAdd
        {
            get
            {
                yield return new object[]
                {
                    (GetFirstLatterLower, GetFirstLatterUpper, new List<List<string>>
                    {
                        new List<string> {"aa", "ba"},
                        new List<string> {"ca", "da"},
                        new List<string> {"bb", "cb"},
                        new List<string>()
                    }),
                    "values:aa,ba,bb,ca,cb,da;keys:A,B,C,D;counts:4,1,2,2,1"
                };
                yield return new object[]
                {
                    (default(Func<string, string>), GetFirstLatterUpper, new List<List<string>>
                    {
                        new List<string> {"aa", "ba"},
                        new List<string> {"ca", "da"},
                        new List<string> {"bb", "cb"},
                        new List<string>()
                    }),
                    "values:aa,ba,bb,ca,cb,da;keys:A,B,C,D;counts:4,1,2,2,1"
                };
                yield return new object[]
                {
                    (GetFirstLatterLower, GetFirstLatterUpper, new List<List<string>>
                    {
                        new List<string> {"aa", "ba"},
                        new List<string> {"ca", "da"},
                        new List<string> {"bb", "cb"},
                        new List<string>()
                    }),
                    "values:aa,ba,bb,ca,cb,da;keys:A,B,C,D;counts:4,1,2,2,1"
                };
            }
        }

        public static IEnumerable<object[]> DataToRemove
        {
            get
            {
                yield return new object[]
                {
                    (GetFirstLatterLower, GetFirstLatterUpper, BuildCollection(), new List<string>
                    {
                        "ab",
                        "ba"
                    }),
                    "values:aa,ab,ba,aa;keys:a,b,A;counts:3,2,1,1"
                };
                yield return new object[]
                {
                    (default(Func<string, string>), GetFirstLatterUpper, BuildCollection(), new List<string>
                    {
                        "ab",
                        "ba"
                    }),
                    "values:aa,ab,ba,aa;keys:a,b,A;counts:3,2,1,1"
                };
                yield return new object[]
                {
                    (GetFirstLatterLower, default(Func<string, string>), BuildCollection(), new List<string>
                    {
                        "ab",
                        "ba"
                    }),
                    "values:aa,aa,ab,ba;keys:a,A,B;counts:3,1,2,1"
                };
            }
        }

        private static ObservableKeyGroupsCollection<string, string> BuildCollection()
        {
            var result = new ObservableKeyGroupsCollection<string, string>(GetFirstLatterLower);
            result.AddRangeToGroups(new[] {"aa", "ab", "ba"});
            result.AddRangeToGroups(new[] {"aa", "ab", "ba"}, GetFirstLatterUpper);
            return result;
        }
    }

    internal static class CollectionSorterTestData
    {
        public static IEnumerable<object[]> SortParams
        {
            get
            {
                yield return new object[]
                {
                    true,
                    new ObservableCollection<string> {"a", "c", "b", "A"},
                    "aAbc"
                };
                yield return new object[]
                {
                    false,
                    new ObservableCollection<string> {"a", "c", "b", "A"},
                    "cbAa"
                };
            }
        }
    }
}