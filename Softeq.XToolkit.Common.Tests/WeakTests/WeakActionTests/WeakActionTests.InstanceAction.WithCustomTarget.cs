// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakActionTests
{
    public partial class WeakActionTests
    {
        [Fact]
        public void IsAlive_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetAlive_AfterGarbageCollection_ReturnsTrue()
        {
            var (_, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetAlive_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction(y));

            GC.Collect();

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void IsAlive_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetAlive_AfterGarbageCollection_ReturnsFalse()
        {
            var (customTarget, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        [Fact]
        public void IsAlive_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetDead_AfterGarbageCollection_ReturnsFalse()
        {
            var (_, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForInstanceAction_WithCustomTargetAlive_WithOriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        [Fact]
        public void IsAlive_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetDead_AfterGarbageCollection_ReturnsFalse()
        {
            var (customTarget, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForInstanceAction_WithCustomTargetDead_WithOriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakAction) = WeakDelegateCreator.CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }
    }
}
