// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyKeyGroupsCollectionChangedEventArgsTests
{
    public class NotifyKeyGroupsCollectionChangedEventArgsTests
    {
        [Theory]
        [MemberData(
            nameof(NotifyKeyGroupsCollectionChangedEventArgsDataProvider.Data),
            MemberType = typeof(NotifyKeyGroupsCollectionChangedEventArgsDataProvider))]
        public void NotifyKeyGroupsCollectionChangedEventArgs_WhenCreated_InitializesProperties(
            NotifyCollectionChangedAction action,
            List<int> oldSectionsSizes)
        {
            var args = new NotifyKeyGroupsCollectionChangedEventArgs(action, oldSectionsSizes);

            oldSectionsSizes ??= new List<int>();

            Assert.Equal(action, args.Action);
            Assert.Equal(oldSectionsSizes, args.OldSectionsSizes);
            Assert.Equal(oldSectionsSizes.Count, args.OldSectionsCount);
            Assert.Empty(args.ModifiedSectionsIndexes);
            Assert.Empty(args.ModifiedItemsIndexes);
        }
    }
}
