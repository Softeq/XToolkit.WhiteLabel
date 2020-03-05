// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.Common.Tests.Extensions.AssemblyExtensionsTests
{
    internal static class AssemblyExtensionsDataProvider
    {
        public static IEnumerable<object[]> AssemblyNamesData
        {
            get
            {
                yield return new object[] { typeof(AssemblyExtensionsTests), "Softeq.XToolkit.Common.Tests" };
                yield return new object[] { typeof(AssemblyExtensions), "Softeq.XToolkit.Common" };
            }
        }
    }
}
