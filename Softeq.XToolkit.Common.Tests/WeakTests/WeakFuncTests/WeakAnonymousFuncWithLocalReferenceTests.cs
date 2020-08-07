// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakFuncTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public class WeakAnonymousFuncWithLocalReferenceTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TOut>(callCounter);

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_AfterGarbageCollection_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TOut>(callCounter);

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TOut>(callCounter);

            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }
    }
}
