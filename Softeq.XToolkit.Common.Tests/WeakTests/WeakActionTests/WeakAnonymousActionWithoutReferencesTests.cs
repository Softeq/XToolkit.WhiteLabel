// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakActionTests
{
    public class WeakAnonymousActionWithoutReferencesTests
    {
        [Fact]
        public void IsStatic_ReturnsFalse()
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences();

            Assert.False(weakAction.IsStatic);
        }

        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        [Fact]
        public void IsAlive_AfterGarbageCollection_ReturnsTrue()
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }
    }
}
