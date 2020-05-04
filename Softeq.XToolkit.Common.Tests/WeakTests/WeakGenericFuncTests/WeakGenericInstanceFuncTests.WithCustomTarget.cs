// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericFuncTests
{
    public partial class WeakGenericInstanceFuncTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetAlive_WithOriginalTargetAlive_AfterGarbageCollection_ReturnsTrue<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, _, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetAlive_WithOriginalTargetAlive_AfterGarbageCollection_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetDead_WithOriginalTargetAlive_AfterGarbageCollection_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (customTarget, _, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetDead_WithOriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetAlive_WithOriginalTargetDead_AfterGarbageCollection_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, originalTarget, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetAlive_WithOriginalTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            originalTarget.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetDead_WithOriginalTargetDead_AfterGarbageCollection_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (customTarget, originalTarget, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetDead_WithOriginalTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakFunc) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }
    }
}
