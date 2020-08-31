// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyKeyGroupsCollectionChangedEventArgsTests
{
    internal static class NotifyKeyGroupsCollectionChangedEventArgsDataProvider
    {
        public static TheoryData<NotifyCollectionChangedAction, List<int>> Data
           => new TheoryData<NotifyCollectionChangedAction, List<int>>
           {
               { NotifyCollectionChangedAction.Add, null },
               { NotifyCollectionChangedAction.Add, new List<int>() },
               { NotifyCollectionChangedAction.Add, new List<int>(){ 1, 2 } },
           };
    }
}
