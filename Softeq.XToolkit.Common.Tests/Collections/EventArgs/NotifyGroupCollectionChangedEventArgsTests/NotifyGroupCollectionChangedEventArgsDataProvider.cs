// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyGroupCollectionChangedEventArgsTests
{
    internal static class NotifyGroupCollectionChangedEventArgsDataProvider
    {
        public static TheoryData<NotifyCollectionChangedAction?,
            IReadOnlyList<(int Index, IReadOnlyList<int> NewItems)>,
            IReadOnlyList<(int Index, IReadOnlyList<int> OldItems)>> Data
           => new TheoryData<NotifyCollectionChangedAction?,
            IReadOnlyList<(int Index, IReadOnlyList<int> NewItems)>,
            IReadOnlyList<(int Index, IReadOnlyList<int> OldItems)>>
           {
               { null, null, null },
               { null, null, new List<(int, IReadOnlyList<int>)>() },
               { null, new List<(int, IReadOnlyList<int>)>(), null },
               { null, new List<(int, IReadOnlyList<int>)>(), new List<(int, IReadOnlyList<int>)>() },
               { NotifyCollectionChangedAction.Add, null, null },
               { NotifyCollectionChangedAction.Add, null, new List<(int, IReadOnlyList<int>)>() },
               { NotifyCollectionChangedAction.Add, new List<(int, IReadOnlyList<int>)>(), null },
               { NotifyCollectionChangedAction.Add, new List<(int, IReadOnlyList<int>)>(), new List<(int, IReadOnlyList<int>)>() }
           };
    }
}
