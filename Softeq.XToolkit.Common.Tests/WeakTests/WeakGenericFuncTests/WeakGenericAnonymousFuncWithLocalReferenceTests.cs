// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericFuncTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public class WeakGenericAnonymousFuncWithLocalReferenceTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_AfterGarbageCollection_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }
    }
}
