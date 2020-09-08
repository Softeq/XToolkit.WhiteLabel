// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupTests
{
    internal static class ObservableKeyGroupDataProvider
    {
        public static TheoryData<string> KeysData
          => new TheoryData<string>
          {
               { null },
               { string.Empty },
               { "abc" },
          };

        public static TheoryData<string, IEnumerable<int>> KeysAndItemsData
          => new TheoryData<string, IEnumerable<int>>
          {
               { null, new List<int>() { 1 } },
               { null, new List<int>() { 1, 2 } },
               { string.Empty, new List<int>() { 1, 2, 3 } },
               { string.Empty, new List<int>() { 1 } },
               { "abc", new List<int>() { 1, 2 } },
               { "abc", new List<int>() { 1, 2, 3 } },
          };
    }
}
