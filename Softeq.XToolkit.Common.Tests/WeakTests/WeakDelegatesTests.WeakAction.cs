// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        #region WeakInstanceAction

        [Fact]
        public void WeakInstanceAction_NotStatic()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        public void WeakInstanceAction_AfterGarbageCollection_WithStrongReference_StillAlive()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceAction_WhenAlive_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction());

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void WeakInstanceAction_AfterGarbageCollection_WithoutStrongReference_NotAlive()
        {
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceAction_WhenNotAlive_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        #endregion

        #region WeakAnonymousActionWithoutReferences

        [Fact]
        public void WeakAnonymousActionWithoutReferences_NotStatic()
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences();

            Assert.False(weakAction.IsStatic);
        }

        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        [Fact]
        public void WeakAnonymousActionWithoutReferences_AfterGarbageCollection_StillAlive()
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        #endregion

        #region WeakAnonymousActionWithInstanceReference

        [Fact]
        public void WeakAnonymousActionWithInstanceReference_NotStatic()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        public void WeakAnonymousActionWithInstanceReference_AfterGarbageCollection_WithStrongReference_StillAlive()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAnonymousActionWithInstanceReference_WhenAlive_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void WeakAnonymousActionWithInstanceReference_AfterGarbageCollection_WithoutStrongReference_NotAlive()
        {
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAnonymousActionWithInstanceReference_WhenNotAlive_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        #endregion

        #region WeakAnonymousActionWithLocalReference

        [Fact]
        public void WeakAnonymousActionWithLocalReference_NotStatic()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference(callCounter);

            Assert.False(weakAction.IsStatic);
        }

        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        [Fact]
        public void WeakAnonymousActionWithLocalReference_AfterGarbageCollection_StillAlive()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference(callCounter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAnonymousActionWithLocalReference_WhenNotAlive_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference(callCounter);

            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        #endregion

        #region WeakStaticInstance

        [Fact]
        public void WeakStaticAction_IsStatic()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

            Assert.True(weakAction.IsStatic);
        }

        [Fact]
        public void WeakStaticAction_AfterGarbageCollection_StillAlive()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakStaticAction_WhenExecuted_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

                weakAction.Execute();

                callCounter.Received(1).OnActionCalled();
            }
        }

        #endregion

        #region WeakAnonymousActionWithStaticReference

        [Fact]
        public void WeakAnonymousActionWithStaticReference_NotStatic()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference();

            Assert.False(weakAction.IsStatic);
        }

        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        [Fact]
        public void WeakAnonymousActionWithStaticReference_AfterGarbageCollection_StillAlive()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAnonymousActionWithStaticReference_WhenExecuted_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference();

                weakAction.Execute();

                callCounter.Received(1).OnActionCalled();
            }
        }

        #endregion

        #region WeakInstanceAction_WithCustomTarget

        [Fact]
        public void WeakInstanceAction_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_StillAlive()
        {
            var (_, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction(y));

            GC.Collect();

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_NotAlive()
        {
            var (customTarget, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_NotAlive()
        {
            var (_, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_NotAlive()
        {
            var (customTarget, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceAction_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        #endregion

        #region WeakStaticAction_WithCustomTarget

        [Fact]
        public void WeakStaticAction_CustomTargetAlive_AfterGarbageCollection_StillAlive()
        {
            var (_, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakStaticAction_CustomTargetAlive_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

                GC.Collect();

                weakAction.Execute();

                callCounter.Received(1).OnActionCalled();
            }
        }

        [Fact]
        public void WeakStaticAction_CustomTargetDead_AfterGarbageCollection_NotAlive()
        {
            var (customTarget, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakStaticAction_CustomTargetDead_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

                customTarget.Dispose();
                GC.Collect();

                weakAction.Execute();

                callCounter.DidNotReceive().OnActionCalled();
            }
        }

        #endregion

        #region MarkForDeletion

        [Fact]
        public void WeakInstanceDelegate_WhenMarkedForDeletion_NotAlive()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakInstanceDelegate_WhenMarkedForDeletion_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction());

            weakAction.MarkForDeletion();
            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        [Fact]
        public void WeakStaticDelegate_WhenMarkedForDeletion_NotAlive()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakStaticDelegate_WhenMarkedForDeletion_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

                weakAction.MarkForDeletion();
                weakAction.Execute();

                callCounter.DidNotReceive().OnActionCalled();
            }
        }

        #endregion
    }
}
