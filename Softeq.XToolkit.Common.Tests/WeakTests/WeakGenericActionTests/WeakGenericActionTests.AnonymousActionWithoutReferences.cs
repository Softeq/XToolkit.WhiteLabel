// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericActionTests
{
    public partial class WeakGenericActionTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ForAnonymousActionWithoutReferences_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences<TIn>();

            Assert.False(weakAction.IsStatic);
        }

        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForAnonymousActionWithoutReferences_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences<TIn>();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }
    }
}
