// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Softeq.XToolkit.Common.Collections.EventArgs;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyKeyGroupCollectionChangedEventArgsTests
{
    internal static class NotifyKeyGroupCollectionChangedEventArgsDataProvider
    {
        public static TheoryData<NotifyCollectionChangedAction?,
            IReadOnlyList<(int Index, IReadOnlyList<int> NewItems)>,
            IReadOnlyList<(int Index, IReadOnlyList<int> OldItems)>,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedEventArgs<int> Arg)>> Data
           => new TheoryData<NotifyCollectionChangedAction?,
            IReadOnlyList<(int Index, IReadOnlyList<int> NewItems)>,
            IReadOnlyList<(int Index, IReadOnlyList<int> OldItems)>,
            IReadOnlyList<(int GroupIndex, NotifyGroupCollectionChangedEventArgs<int> Arg)>>
           {
               { null, null, null, null },
               { null, null, new List<(int, IReadOnlyList<int>)>(), null },
               { null, new List<(int, IReadOnlyList<int>)>(), null, null },
               { null, new List<(int, IReadOnlyList<int>)>(), new List<(int, IReadOnlyList<int>)>(), null },
               { NotifyCollectionChangedAction.Add, null, null, null },
               { NotifyCollectionChangedAction.Add, null, new List<(int, IReadOnlyList<int>)>(), null },
               { NotifyCollectionChangedAction.Add, new List<(int, IReadOnlyList<int>)>(), null, null },
               { NotifyCollectionChangedAction.Add, new List<(int, IReadOnlyList<int>)>(), new List<(int, IReadOnlyList<int>)>(), null },
               { null, null, null, new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { null, null, new List<(int, IReadOnlyList<int>)>(), new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { null, new List<(int, IReadOnlyList<int>)>(), null, new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { null, new List<(int, IReadOnlyList<int>)>(), new List<(int, IReadOnlyList<int>)>(), new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { NotifyCollectionChangedAction.Add, null, null, new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { NotifyCollectionChangedAction.Add, null, new List<(int, IReadOnlyList<int>)>(), new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { NotifyCollectionChangedAction.Add, new List<(int, IReadOnlyList<int>)>(), null, new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() },
               { NotifyCollectionChangedAction.Add, new List<(int, IReadOnlyList<int>)>(), new List<(int, IReadOnlyList<int>)>(), new List<(int, NotifyGroupCollectionChangedEventArgs<int>)>() }
           };
    }
}
