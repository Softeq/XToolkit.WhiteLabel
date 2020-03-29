using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Weak;

namespace Softeq.XToolkit.Common.Tests.WeakTests.Utils
{
    public static class WeakActionGenerator
    {
        public static IEnumerable<WeakAction> Generate(Func<IActionRunner> instanceFactory)
        {
            yield return new WeakAction();
        }
    }
}
