// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

#pragma warning disable SA1509

namespace Softeq.XToolkit.Common.Tests.Extensions.ObservableCollectionExtensionsTests
{
    internal static class ObservableCollectionDataProvider
    {
        private static readonly ObservableCollection<int> EmptyCollection
            = new ObservableCollection<int>();

        private static readonly ObservableCollection<int> EqualsCollection
            = new ObservableCollection<int> { 1, 1, 1, 1, 1, 1, 1 };

        private static readonly ObservableCollection<int> AscendingSortedCollection
            = new ObservableCollection<int> { -8, -5, -2, 0, 0, 1, 1, 2, 3, 4, 4, 4, 5, 6, 7 };

        private static readonly ObservableCollection<int> DescendingSortedCollection
            = new ObservableCollection<int> { 7, 6, 5, 4, 4, 4, 3, 2, 1, 1, 0, 0, -2, -5, -8 };

        private static readonly ObservableCollection<int> NonSortedCollection1
            = new ObservableCollection<int> { 4, -5, 1, 6, -2, 7, 4, 1, 3, 0, 4, 2, -8, 5, 0 };

        private static readonly ObservableCollection<int> NonSortedCollection2
            = new ObservableCollection<int> { 15, -9, 57, 14, 0, -2, -124, 96, 73, 194, -55, 0, 11 };

        private static readonly Comparer<int> AscComparer = Comparer<int>.Create(AscComparison);
        private static readonly Comparer<int> DescComparer = Comparer<int>.Create(DescComparison);
        private static readonly Comparer<int> AbsComparer = Comparer<int>.Create(AbsComparison);
        private static readonly Comparer<int> CustomComparer = Comparer<int>.Create(CustomComparison);

        public static TheoryData<ObservableCollection<int>> CollectionsData
            => new TheoryData<ObservableCollection<int>>
            {
                { EmptyCollection },
                { EqualsCollection },
                { AscendingSortedCollection },
                { DescendingSortedCollection },
                { NonSortedCollection1 },
                { NonSortedCollection2 },
            };

        public static TheoryData<ObservableCollection<int>, Comparison<int>> ComparisonsData
            => new TheoryData<ObservableCollection<int>, Comparison<int>>
            {
                { EmptyCollection, AscComparison },
                { EmptyCollection, DescComparison },
                { EmptyCollection, AbsComparison },
                { EmptyCollection, CustomComparison },

                { EqualsCollection, AscComparison },
                { EqualsCollection, DescComparison },
                { EqualsCollection, AbsComparison },
                { EqualsCollection, CustomComparison },

                { AscendingSortedCollection, AscComparison },
                { AscendingSortedCollection, DescComparison },
                { AscendingSortedCollection, AbsComparison },
                { AscendingSortedCollection, CustomComparison },

                { DescendingSortedCollection, AscComparison },
                { DescendingSortedCollection, DescComparison },
                { DescendingSortedCollection, AbsComparison },
                { DescendingSortedCollection, CustomComparison },

                { NonSortedCollection1, AscComparison },
                { NonSortedCollection1, DescComparison },
                { NonSortedCollection1, AbsComparison },
                { NonSortedCollection1, CustomComparison },

                { NonSortedCollection2, AscComparison },
                { NonSortedCollection2, DescComparison },
                { NonSortedCollection2, AbsComparison },
                { NonSortedCollection2, CustomComparison },
            };

        public static TheoryData<ObservableCollection<int>, Comparison<int>> WrongComparisonsData
            => new TheoryData<ObservableCollection<int>, Comparison<int>>
            {
                { null, AscComparison },
                { null, CustomComparison },
                { EqualsCollection, null },
                { NonSortedCollection1, null },
                { null, null }
            };

        public static TheoryData<ObservableCollection<int>, IComparer<int>> ComparersData
            => new TheoryData<ObservableCollection<int>, IComparer<int>>
            {
                { EmptyCollection, AscComparer },
                { EmptyCollection, DescComparer },
                { EmptyCollection, AbsComparer },
                { EmptyCollection, CustomComparer },

                { EqualsCollection, AscComparer },
                { EqualsCollection, DescComparer },
                { EqualsCollection, AbsComparer },
                { EqualsCollection, CustomComparer },

                { AscendingSortedCollection, AscComparer },
                { AscendingSortedCollection, DescComparer },
                { AscendingSortedCollection, AbsComparer },
                { AscendingSortedCollection, CustomComparer },

                { DescendingSortedCollection, AscComparer },
                { DescendingSortedCollection, DescComparer },
                { DescendingSortedCollection, AbsComparer },
                { DescendingSortedCollection, CustomComparer },

                { NonSortedCollection1, AscComparer },
                { NonSortedCollection1, DescComparer },
                { NonSortedCollection1, AbsComparer },
                { NonSortedCollection1, CustomComparer },

                { NonSortedCollection2, AscComparer },
                { NonSortedCollection2, DescComparer },
                { NonSortedCollection2, AbsComparer },
                { NonSortedCollection2, CustomComparer },
            };

        public static TheoryData<ObservableCollection<int>, IComparer<int>> WrongComparersData
            => new TheoryData<ObservableCollection<int>, IComparer<int>>
            {
                { null, AscComparer },
                { null, CustomComparer },
                { EqualsCollection, null },
                { NonSortedCollection1, null },
                { null, null }
            };

        private static int AscComparison(int a, int b) => a.CompareTo(b);
        private static int DescComparison(int a, int b) => b.CompareTo(a);
        private static int AbsComparison(int a, int b) => Math.Abs(a).CompareTo(Math.Abs(b));
        private static int CustomComparison(int a, int b) => a == 0 ? 1 : a.CompareTo(b);
    }
}
