// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyGroupCollectionChangedEventArgsTests
{
    public class NotifyGroupCollectionChangedEventArgsTests
    {
        [Theory]
        [MemberData(
            nameof(NotifyGroupCollectionChangedEventArgsDataProvider.Data),
            MemberType = typeof(NotifyGroupCollectionChangedEventArgsDataProvider))]
        public void NotifyGroupCollectionChangedEventArgs_WhenCreated_InitializesProperties(
            NotifyCollectionChangedAction? action,
            IReadOnlyList<(int Index, IReadOnlyList<int> NewItems)> newItems,
            IReadOnlyList<(int Index, IReadOnlyList<int> OldItems)> oldItems)
        {
            var args = new NotifyGroupCollectionChangedEventArgs<int>(action, newItems, oldItems);

            Assert.Equal(action, args.Action);
            Assert.Equal(newItems, args.NewItemRanges);
            Assert.Equal(oldItems, args.OldItemRanges);
        }
    }
}
