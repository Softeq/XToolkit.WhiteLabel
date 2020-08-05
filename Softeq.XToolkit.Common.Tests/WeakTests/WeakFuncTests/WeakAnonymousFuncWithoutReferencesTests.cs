// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakFuncTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public class WeakAnonymousFuncWithoutReferencesTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var weakFunc = WeakDelegatesCallCounter
                .GetWeakAnonymousFuncWithoutReferences<TOut>();

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_AfterGarbageCollection_ReturnsTrue<TOut>(TOut outputParameter)
        {
            var weakFunc = WeakDelegatesCallCounter
                .GetWeakAnonymousFuncWithoutReferences<TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }
    }
}
