// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.ObservableCollectionExtensionsTests
{
    public class ObservableCollectionExtensionsTests
    {
        [Theory]
        [MemberData(nameof(ObservableCollectionDataProvider.CollectionsData), MemberType = typeof(ObservableCollectionDataProvider))]
        public void Sort_WithoutComparison_SortsObservableCollection(
            ObservableCollection<int> data)
        {
            data.Sort();

            var sortedList = GetSortedList(data);
            Assert.True(CheckCollectionSorted(data, sortedList));
        }

        [Fact]
        public void Sort_OnNullCollection_ThrowsException()
        {
            ObservableCollection<int> data = null;
            Assert.Throws<ArgumentNullException>(() => data.Sort());
        }

        [Theory]
        [MemberData(nameof(ObservableCollectionDataProvider.ComparisonsData), MemberType = typeof(ObservableCollectionDataProvider))]
        public void Sort_UsingValidComparison_SortsObservableCollection(
            ObservableCollection<int> data, Comparison<int> comparison)
        {
            data.Sort(comparison);

            var sortedList = GetSortedList(data, comparison);
            Assert.True(CheckCollectionSorted(data, sortedList));
        }

        [Theory]
        [MemberData(nameof(ObservableCollectionDataProvider.WrongComparisonsData), MemberType = typeof(ObservableCollectionDataProvider))]
        public void Sort_OnNullCollectionOrNullComparison_ThrowsException(
            ObservableCollection<int> data, Comparison<int> comparison)
        {
            Assert.Throws<ArgumentNullException>(() => data.Sort(comparison));
        }

        [Theory]
        [MemberData(nameof(ObservableCollectionDataProvider.ComparersData), MemberType = typeof(ObservableCollectionDataProvider))]
        public void Sort_UsingValidComparer_SortsObservableCollection(
            ObservableCollection<int> data, IComparer<int> comparer)
        {
            data.Sort(comparer);

            var sortedList = GetSortedList(data, comparer);
            Assert.True(CheckCollectionSorted(data, sortedList));
        }

        [Theory]
        [MemberData(nameof(ObservableCollectionDataProvider.WrongComparersData), MemberType = typeof(ObservableCollectionDataProvider))]
        public void Sort_OnNullCollectionOrNullComparer_ThrowsException(
            ObservableCollection<int> data, IComparer<int> comparer)
        {
            Assert.Throws<ArgumentNullException>(() => data.Sort(comparer));
        }

        private List<T> GetSortedList<T>(ObservableCollection<T> data)
        {
            var sortedList = data.ToList();
            sortedList.Sort();
            return sortedList;
        }

        private List<T> GetSortedList<T>(ObservableCollection<T> data, Comparison<T> comparison)
        {
            var sortedList = data.ToList();
            sortedList.Sort(comparison);
            return sortedList;
        }

        private List<T> GetSortedList<T>(ObservableCollection<T> data, IComparer<T> comparer)
        {
            var sortedList = data.ToList();
            sortedList.Sort(comparer);
            return sortedList;
        }

        private bool CheckCollectionSorted(ObservableCollection<int> data, List<int> sortedList)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] != sortedList[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
