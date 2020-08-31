// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyCollectionInsertEventArgsTests
{
    public class NotifyCollectionInsertEventArgsTests
    {
        [Theory]
        [MemberData(
            nameof(NotifyCollectionInsertEventArgsDataProvider.Data),
            MemberType = typeof(NotifyCollectionInsertEventArgsDataProvider))]
        public void NotifyCollectionInsertEventArgs_WhenCreated_SetsActionAndInsertedItemsIndexes(
            List<int> insertedItemsIndexes)
        {
            var args = new NotifyCollectionInsertEventArgs(insertedItemsIndexes);

            if (insertedItemsIndexes == null)
            {
                insertedItemsIndexes = new List<int>();
            }

            Assert.Equal(NotifyCollectionChangedAction.Add, args.Action);
            Assert.Equal(insertedItemsIndexes, args.InsertedItemsIndexes);
            Assert.Empty(args.NewItems);
            Assert.Null(args.OldItems);
            Assert.Equal(-1, args.NewStartingIndex);
            Assert.Equal(-1, args.OldStartingIndex);
        }
    }
}
