// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        public static IEnumerable<object[]> WeakActionInputParameters => TypesGenerator().Select(x => new[] { x });
        public static IEnumerable<object[]> WeakFuncOutputParameters => TypesGenerator().Select(x => new[] { x });
        public static IEnumerable<object[]> WeakFuncInputOutputParameters =>
            TypesGenerator().SelectMany(t => TypesGenerator(), (x, y) => new[] { x, y });

        private static IEnumerable<object> TypesGenerator()
        {
            yield return 42;    // int
            yield return 42.0;  // double
            yield return "42";  // string
        }
    }
}
