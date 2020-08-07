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
    [Collection(nameof(StaticWeakDelegatesCallCounter))]
    public class WeakGenericAnonymousFuncWithStaticReferenceTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TIn, TOut>();

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_AfterGarbageCollection_ReturnsTrue<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TIn, TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_AfterGarbageCollection_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TIn, TOut>();

                weakFunc.Execute(inputParameter);

                callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
            }
        }
    }
}
