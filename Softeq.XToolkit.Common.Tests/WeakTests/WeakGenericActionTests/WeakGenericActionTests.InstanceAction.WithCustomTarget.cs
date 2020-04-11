// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericActionTests
{
    public partial class WeakGenericActionTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetAlive_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var (_, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetAlive_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetAlive_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (customTarget, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetDead_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (_, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetDead_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (customTarget, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }
    }
}
