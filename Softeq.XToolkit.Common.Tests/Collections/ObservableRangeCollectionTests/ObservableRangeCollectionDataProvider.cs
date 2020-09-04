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

        #region AddRange

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction> ValidRangeWithWrongNotificationModesForAdding
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Move },
               { new List<int>(), NotifyCollectionChangedAction.Remove },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction> ValidRangeWithWrongNotificationModesForRemoving
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new List<int>() { 1 }, NotifyCollectionChangedAction.Move },
               { new List<int>(), NotifyCollectionChangedAction.Add },
               { new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<IEnumerable<int>, NotifyCollectionChangedAction> NullRangeWithAnyNotificationMode
           => new TheoryData<IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { null, NotifyCollectionChangedAction.Add },
               { null, NotifyCollectionChangedAction.Reset },
               { null, NotifyCollectionChangedAction.Move },
               { null, NotifyCollectionChangedAction.Remove },
               { null, NotifyCollectionChangedAction.Replace },
           };

        #endregion

        #region InsertRange

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>> ValidInitilalCollectionWithValidRange
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

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int> NullRangeWithAnyStartIndex
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>(), null, 0 },
               { new ObservableRangeCollection<int>(), null, 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, null, 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, null, 6 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int> ValidRangeWithInvalidStartIndex
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2 }, 2 },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2 }, -1 },
               { new ObservableRangeCollection<int>(), new List<int>(), 3 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 6 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, -2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>(), 7 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int> ValidRangeWithValidStartIndex
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, int>
           {
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2 }, 0 },
               { new ObservableRangeCollection<int>(), new List<int>(), 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 4 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>() { 5, 6 }, 2 },
               { new ObservableRangeCollection<int>() { 1, 2, 3, 4 }, new List<int>(), 3 },
           };

        #endregion

        #region Replace

        public static TheoryData<ObservableRangeCollection<int>, int> ReplaceData
           => new TheoryData<ObservableRangeCollection<int>, int>
           {
               { new ObservableRangeCollection<int>(), 0 },
               { new ObservableRangeCollection<int>() { 3 }, 0 },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, 0 },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>> ReplaceRangeData
          => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>>
          {
               { new ObservableRangeCollection<int>(), new List<int>() },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2, 3 } },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 } },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 4, 5, 6 } },
          };

        #endregion

        #region InsertRangeSorted

        public static TheoryData<ObservableRangeCollection<int>, Comparison<int>, NotifyCollectionChangedAction> InsertRangeSortedNullRangeTestData
           => new TheoryData<ObservableRangeCollection<int>, Comparison<int>, NotifyCollectionChangedAction>
           {
               { new ObservableRangeCollection<int>(), AscComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>(), DescComparison, NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>(), AbsComparison, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 3 }, CustomComparison, NotifyCollectionChangedAction.Replace },
               { new ObservableRangeCollection<int>() { 3 }, AscComparison, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 3 }, DescComparison, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, AbsComparison, NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, CustomComparison, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, AscComparison, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, NotifyCollectionChangedAction> InsertRangeSortedNullComparisonTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, NotifyCollectionChangedAction>
           {
               { new ObservableRangeCollection<int>(), new List<int>(), NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>(), new List<int>() { 1 }, NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>(), NotifyCollectionChangedAction.Replace },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1 }, NotifyCollectionChangedAction.Reset },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Add },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>(), NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1 }, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>, NotifyCollectionChangedAction> InsertRangeSortedWrongNotificationModeTestData
           => new TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>, NotifyCollectionChangedAction>
           {
               { new ObservableRangeCollection<int>(), new List<int>(), AscComparison, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>(), new List<int>() { 1 }, DescComparison, NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>(), new List<int>() { 1, 2, 3 }, AbsComparison, NotifyCollectionChangedAction.Replace },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>(), CustomComparison, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1 }, AscComparison,  NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>() { 3 }, new List<int>() { 1, 2, 3 }, DescComparison,  NotifyCollectionChangedAction.Replace },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>(), AbsComparison, NotifyCollectionChangedAction.Remove },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1 }, CustomComparison, NotifyCollectionChangedAction.Move },
               { new ObservableRangeCollection<int>() { 1, 2, 3 }, new List<int>() { 1, 2, 3 }, AscComparison, NotifyCollectionChangedAction.Replace },
           };

        public static TheoryData<ObservableRangeCollection<int>, IEnumerable<int>, Comparison<int>> InsertRangeSortedEverythingValidTestData
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

        #endregion

        private static int AscComparison(int a, int b) => a.CompareTo(b);
        private static int DescComparison(int a, int b) => b.CompareTo(a);
        private static int AbsComparison(int a, int b) => Math.Abs(a).CompareTo(Math.Abs(b));
        private static int CustomComparison(int a, int b) => a == 0 ? 1 : a.CompareTo(b);
    }
}
