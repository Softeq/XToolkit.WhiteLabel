// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableRangeCollectionTests
{
    internal static class ObservableRangeCollectionDataProvider
    {
        public static TheoryData<IEnumerable<int>> InitData
           => new TheoryData<IEnumerable<int>>
           {
               { new List<int>() { 1 } },
               { new List<int>() { 1, 2 } },
               { new List<int>() { 1, 2, 3 } },
           };

        public static TheoryData<ObservableRangeCollection<int>> CollectionData
           => new TheoryData<ObservableRangeCollection<int>>
           {
               { new ObservableRangeCollection<int>() },
               { new ObservableRangeCollection<int>() { 1 } },
               { new ObservableRangeCollection<int>() { 1, 2 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3 } },
           };

        public static TheoryData<ObservableRangeCollection<int>> NonEmptyCollectionData
           => new TheoryData<ObservableRangeCollection<int>>
           {
               { new ObservableRangeCollection<int>() { 1 } },
               { new ObservableRangeCollection<int>() { 1, 2 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3 } },
           };

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
            AddRangeValidRangeWithWrongModesTestData
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Move },
               { new List<int>(), NotifyCollectionChangedAction.Remove },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
            RemoveRangeValidRangeWithWrongModesTestData
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Move },
               { new List<int>(), NotifyCollectionChangedAction.Add },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
            AddRangeNonEmptyRangeWithValidModesTestData
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Add },
               { new List<int>() { 1, 2 }, NotifyCollectionChangedAction.Reset },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Add },
               { new List<int>() { 1, 2, 3, 4 }, NotifyCollectionChangedAction.Reset },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, NotifyCollectionChangedAction>
            RemoveRangeNonEmptyCollectionWithValidRangeAndValidModeWithNoItemsToRemoveTestData
          => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, NotifyCollectionChangedAction>
          {
               { new ObservableRangeCollection<int>() { 1 }, new List<int>(), NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 2 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 2, 3, 4 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>(), NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() { 5 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() { 3, 4 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4, 5 }, new List<int>(), NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4, 5 }, new List<int>() { 32, -2, 0, -2 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4, 5 }, new List<int>() { -5, 18, -4, -1, 0 }, NotifyCollectionChangedAction.Remove },
          };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, NotifyCollectionChangedAction>
            RemoveRangeNonEmptyCollectionWithValidRangeAndValidModeWithItemsToRemoveTestData
          => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, NotifyCollectionChangedAction>
          {
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1, 2, 3, 4 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() { 1 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() { 1, 2, 3, 4 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 2, 4, 5 }, new List<int>() { 3, 2, 2, 4 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2, 2, 4, 5 }, new List<int>() { 5, 18, 4, 1, 0 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 2, 2, 5 }, new List<int>() { 2, 2, 2, 2 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2, 2, 2, 5 }, new List<int>() { 1, 1, 2, 1, 0 }, NotifyCollectionChangedAction.Reset },
          };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
            AddOrRemoveRangeValidInitilalCollectionWithValidRangeTestData
          => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
          {
               { new ObservableRangeCollection<int>(), new List<int>() },
               { new ObservableRangeCollection<int>(), new List<int>() { 1 } },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2, 3, 4 } },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1 } },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1, 2, 3, 4 } },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() { 1 } },
               { new ObservableRangeCollection<int>() { 1, 2 }, new List<int>() { 1, 2, 3, 4 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4, 5 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4, 5 }, new List<int>() { 3, 2, 1, 4 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4, 5 }, new List<int>() { 5, 18, 4, 1, 0 } },
          };

        public static TheoryData<ObservableRangeCollection<int>, int>
            InsertRangeNullRangeTestData
           => new TheoryData<ObservableRangeCollection<int>, int>
           {
               { new ObservableRangeCollection<int>(), 0 },
               { new ObservableRangeCollection<int>(), 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, 6 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
            InsertRangeValidRangeWithInvalidStartIndexTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2 }, 2 },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2 }, -1 },
               { new ObservableRangeCollection<int>(), new List<int>(), 3 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 6 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, -2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>(), 7 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
            InsertRangeNonEmptyCollectionWithValidRangeWithInvalidStartIndexTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1, 2 }, 2 },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1, 2 }, -1 },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>(), 3 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 6 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, -2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>(), 7 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
            InsertRangeValidRangeWithValidStartIndexTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2 }, 0 },
               { new ObservableRangeCollection<int>(), new List<int>(), 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 4 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>(), 3 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
            InsertRangeNonEmptyCollectionWithNonEmptyRangeWithValidStartIndexTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1 }, 0 },
               { new ObservableRangeCollection<int>() { 1 }, new List<int>() { 1, 2 }, 1 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 4 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 1, 2, 3, 4 }, 3 },
           };

        public static TheoryData<ObservableRangeCollection<int>, int> ReplaceTestData
           => new TheoryData<ObservableRangeCollection<int>, int>
           {
               { new ObservableRangeCollection<int>(), 0 },
               { new ObservableRangeCollection<int>() { 3 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, 0 },
           };

        public static TheoryData<ObservableRangeCollection<int>, int>
            ReplaceNonEmptyCollectionTestData
           => new TheoryData<ObservableRangeCollection<int>, int>
           {
               { new ObservableRangeCollection<int>() { 1 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, 0 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
            ReplaceRangeTestData
          => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
          {
               { new ObservableRangeCollection<int>(), new List<int>() },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2, 3 } },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 4, 5, 6 } },
          };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
            ReplaceRangeNonEmptyCollectionTestData
          => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
          {
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 } },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 4, 5, 6 } },
          };

        public static TheoryData<Comparison<int>, NotifyCollectionChangedAction>
            InsertRangeSortedNullRangeTestData
           => new TheoryData<Comparison<int>, NotifyCollectionChangedAction>
           {
               { AscComparison, NotifyCollectionChangedAction.Add },
               { DescComparison, NotifyCollectionChangedAction.Move },
               { AbsComparison, NotifyCollectionChangedAction.Remove },
               { CustomComparison, NotifyCollectionChangedAction.Replace },
               { AscComparison, NotifyCollectionChangedAction.Reset },
               { DescComparison, NotifyCollectionChangedAction.Add },
               { AbsComparison, NotifyCollectionChangedAction.Move },
               { CustomComparison, NotifyCollectionChangedAction.Remove },
               { AscComparison, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
            InsertRangeSortedNullComparisonTestData
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new List<int>(), NotifyCollectionChangedAction.Add },
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Move },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Remove },
               { new List<int>(), NotifyCollectionChangedAction.Replace },
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Reset },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Add },
               { new List<int>(), NotifyCollectionChangedAction.Move },
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Remove },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<IEnumerable<int>, Comparison<int>, NotifyCollectionChangedAction>
            InsertRangeSortedWrongModeTestData
           => new TheoryData<IEnumerable<int>, Comparison<int>, NotifyCollectionChangedAction>
           {
               { new List<int>(), AscComparison, NotifyCollectionChangedAction.Remove },
               { new List<int>() { 1 }, DescComparison, NotifyCollectionChangedAction.Move },
               { new List<int>() { 1, 2, 3 }, AbsComparison, NotifyCollectionChangedAction.Replace },
               { new List<int>(), CustomComparison, NotifyCollectionChangedAction.Remove },
               { new List<int>() { 1 }, AscComparison,  NotifyCollectionChangedAction.Move },
               { new List<int>() { 1, 2, 3 }, DescComparison,  NotifyCollectionChangedAction.Replace },
               { new List<int>(), AbsComparison, NotifyCollectionChangedAction.Remove },
               { new List<int>() { 1 }, CustomComparison, NotifyCollectionChangedAction.Move },
               { new List<int>() { 1, 2, 3 }, AscComparison, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>>
            InsertRangeSortedEverythingValidTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>>
           {
               { new ObservableRangeCollection<int>(), new List<int>(), AscComparison },
               { new ObservableRangeCollection<int>(), new List<int>() { 1 }, DescComparison },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2, 3 }, AbsComparison },
               { new ObservableRangeCollection<int>(), new List<int>() { 5, -2, 1 }, CustomComparison },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 5, -2, 1 }, AscComparison },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 }, DescComparison },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1 }, AbsComparison },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>(), CustomComparison },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1 }, AscComparison },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>(), DescComparison },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 5, -2, 1 }, AbsComparison },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, CustomComparison },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 1, 2, 3 }, AscComparison },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 5, -2, 1 }, DescComparison },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>(), AbsComparison },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 1 }, CustomComparison },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>, NotifyCollectionChangedAction>
            InsertRangeSortedNonEmptyCollectionWithNonEmptyRangeWithValidComparisonWithValidModeTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>, NotifyCollectionChangedAction>
           {
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 5, -2, 1 }, AscComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 }, DescComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int> { 3 }, new List<int>() { 1 }, AbsComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 0 }, CustomComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1 }, AscComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 0 }, DescComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 5, -2, 1 }, AbsComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, CustomComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 1, 2, 3 }, AscComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 5, -2, 1 }, DescComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 0 }, AbsComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, new List<int>() { 1 }, CustomComparison, NotifyCollectionChangedAction.Reset },
           };

        public static TheoryData<ObservableRangeCollection<int>, Comparison<int>, NotifyCollectionChangedAction>
            InsertRangeSortedNonEmptyCollectionWithEmptyRangeWithValidComparisonWithValidModeTestData
           => new TheoryData<ObservableRangeCollection<int>, Comparison<int>, NotifyCollectionChangedAction>
           {
               { new ObservableRangeCollection<int>() { 3 }, AscComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 3 }, DescComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, AbsComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, CustomComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, AscComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, DescComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, AbsComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 5, -8, 4 }, CustomComparison, NotifyCollectionChangedAction.Reset },
           };

        private static int AscComparison(int a, int b) => a.CompareTo(b);
        private static int DescComparison(int a, int b) => b.CompareTo(a);
        private static int AbsComparison(int a, int b) => Math.Abs(a).CompareTo(Math.Abs(b));
        private static int CustomComparison(int a, int b) => a == 0 ? 1 : a.CompareTo(b);
    }
}
