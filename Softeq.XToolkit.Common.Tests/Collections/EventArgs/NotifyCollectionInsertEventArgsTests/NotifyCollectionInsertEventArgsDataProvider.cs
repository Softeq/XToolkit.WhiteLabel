// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.EventArgs.NotifyCollectionInsertEventArgsTests
{
    internal static class NotifyCollectionInsertEventArgsDataProvider
    {
        public static TheoryData<List<int>> Data
           => new TheoryData<List<int>>
           {
                { null },
                { new List<int>() },
                { new List<int>() { 1, 2, 3 } },
           };
    }
}
