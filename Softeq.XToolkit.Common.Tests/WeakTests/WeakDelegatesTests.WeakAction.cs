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

        [Fact]
        public void WeakInstanceAction_NotStatic()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        public void WeakInstanceAction_AfterGarbageCollection_WithStrongReference_IsAlive()
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
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithoutReferences());

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        public void WeakAnonymousActionWithoutReferences_AfterGarbageCollection_WithStrongReference_IsAlive()
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithoutReferences());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        public void WeakAnonymousActionWithoutReferences_AfterGarbageCollection_WithoutStrongReference_StillAlive()
        {
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithoutReferences());

            reference.Dispose();
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
        public void WeakAnonymousActionWithInstanceReference_AfterGarbageCollection_WithStrongReference_IsAlive()
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
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference(callCounter));

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        public void WeakAnonymousActionWithLocalReference_AfterGarbageCollection_WithStrongReference_IsAlive()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference(callCounter));

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAnonymousActionWithLocalReference_WhenAlive_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference(callCounter));

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void WeakAnonymousActionWithLocalReference_WhenNotAlive_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference(callCounter));

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
        public void WeakStaticAction_AfterGarbageCollection_IsAlive()
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

        [Fact]
        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
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
    }
}
