// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        #region WeakInstanceAction

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_NotStatic<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_AfterGarbageCollection_WithStrongReference_StillAlive<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_WhenAlive_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction<TIn>());

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_AfterGarbageCollection_WithoutStrongReference_NotAlive<TIn>(TIn inputParameter)
        {
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_WhenNotAlive_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction<TIn>());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        #endregion

        #region WeakAnonymousActionWithoutReferences

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithoutReferences_NotStatic<TIn>(TIn inputParameter)
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences<TIn>();

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        public void Generic_WeakAnonymousActionWithoutReferences_AfterGarbageCollection_StillAlive<TIn>(TIn inputParameter)
        {
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithoutReferences<TIn>();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        #endregion

        #region WeakAnonymousActionWithInstanceReference

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithInstanceReference_NotStatic<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithInstanceReference_AfterGarbageCollection_WithStrongReference_StillAlive<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithInstanceReference_WhenAlive_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithInstanceReference_AfterGarbageCollection_WithoutStrongReference_NotAlive<TIn>(TIn inputParameter)
        {
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithInstanceReference_WhenNotAlive_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        #endregion

        #region WeakAnonymousActionWithLocalReference

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithLocalReference_NotStatic<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter);

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        public void Generic_WeakAnonymousActionWithLocalReference_AfterGarbageCollection_StillAlive<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithLocalReference_WhenNotAlive_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter);

            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        #endregion

        #region WeakStaticInstance

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_IsStatic<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

            Assert.True(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_AfterGarbageCollection_StillAlive<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_WhenExecuted_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

                weakAction.Execute(inputParameter);

                callCounter.Received(1).OnActionCalled(inputParameter);
            }
        }

        #endregion

        #region WeakAnonymousActionWithStaticReference

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithStaticReference_NotStatic<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference<TIn>();

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        public void Generic_WeakAnonymousActionWithStaticReference_AfterGarbageCollection_StillAlive<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference<TIn>();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithStaticReference_WhenExecuted_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference<TIn>();

                weakAction.Execute(inputParameter);

                callCounter.Received(1).OnActionCalled(inputParameter);
            }
        }

        #endregion

        #region WeakInstanceAction_WithCustomTarget

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_StillAlive<TIn>(TIn inputParameter)
        {
            var (_, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x,y) => x.GetWeakInstanceAction<TIn>(y));

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_NotAlive<TIn>(TIn inputParameter)
        {
            var (customTarget, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_NotAlive<TIn>(TIn inputParameter)
        {
            var (_, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceAction<TIn>(y));

            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_NotAlive<TIn>(TIn inputParameter)
        {
            var (customTarget, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x,y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceAction_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakAction) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x,y) => x.GetWeakInstanceAction<TIn>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        #endregion

        #region WeakStaticAction_WithCustomTarget

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_CustomTargetAlive_AfterGarbageCollection_StillAlive<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_CustomTargetAlive_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

                GC.Collect();

                weakAction.Execute(inputParameter);

                callCounter.Received(1).OnActionCalled(inputParameter);
            }
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_CustomTargetDead_AfterGarbageCollection_NotAlive<TIn>(TIn inputParameter)
        {
            var (customTarget, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticAction_CustomTargetDead_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakAction) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

                customTarget.Dispose();
                GC.Collect();

                weakAction.Execute(inputParameter);

                callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
            }
        }

        #endregion

        #region MarkForDeletion

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceDelegate_WhenMarkedForDeletion_NotAlive<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakInstanceDelegate_WhenMarkedForDeletion_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction<TIn>());

            weakAction.MarkForDeletion();
            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticDelegate_WhenMarkedForDeletion_NotAlive<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakStaticDelegate_WhenMarkedForDeletion_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

                weakAction.MarkForDeletion();
                weakAction.Execute(inputParameter);

                callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
            }
        }

        #endregion
    }
}
