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
    public partial class WeakGenericStaticFuncTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsTrue<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

            Assert.True(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_AfterGarbageCollection_ReturnsTrue<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

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
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

                GC.Collect();
                weakFunc.Execute(inputParameter);

                callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
            }
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WhenMarkedForDeletion_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

            weakFunc.MarkForDeletion();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WhenMarkedForDeletion_DoesNotInvokeAction<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

                weakFunc.MarkForDeletion();
                weakFunc.Execute(inputParameter);

                callCounter.DidNotReceive().OnActionCalled();
            }
        }
    }
}
