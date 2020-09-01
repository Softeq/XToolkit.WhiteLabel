// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyKeyGroupCollectionChangedEventArgsTests
{
    public class NotifyKeyGroupCollectionChangedEventArgsTests
    {
        [Theory]
        [MemberData(
            nameof(NotifyKeyGroupCollectionChangedEventArgsDataProvider.Data),
            MemberType = typeof(NotifyKeyGroupCollectionChangedEventArgsDataProvider))]
        public void NotifyKeyGroupCollectionChangedEventArgs_WhenCreated_InitializesProperties(
            NotifyCollectionChangedAction action,
            IReadOnlyList<(int Index, IReadOnlyList<int> NewItems)> newItems,
            IReadOnlyList<(int Index, IReadOnlyList<int> OldItems)> oldItems,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedEventArgs<int> Arg)> groupEvents)
        {
            var args = new NotifyKeyGroupCollectionChangedEventArgs<int, int>(action, newItems, oldItems, groupEvents);

            Assert.Equal(action, args.Action);
            Assert.Equal(newItems, args.NewItemRanges);
            Assert.Equal(oldItems, args.OldItemRanges);
            Assert.Equal(groupEvents, args.GroupEvents);
        }
    }
}
