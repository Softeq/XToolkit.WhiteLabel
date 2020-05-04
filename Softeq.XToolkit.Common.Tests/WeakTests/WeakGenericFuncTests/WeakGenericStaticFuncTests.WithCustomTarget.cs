// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericFuncTests
{
    public partial class WeakGenericStaticFuncTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetAlive_AfterGarbageCollection_ReturnsTrue<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetAlive_AfterGarbageCollection_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakFunc) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

                GC.Collect();

                weakFunc.Execute(inputParameter);

                callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
            }
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetDead_AfterGarbageCollection_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (customTarget, weakFunc) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakFunc) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

                customTarget.Dispose();
                GC.Collect();

                weakFunc.Execute(inputParameter);

                callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
            }
        }
    }
}
