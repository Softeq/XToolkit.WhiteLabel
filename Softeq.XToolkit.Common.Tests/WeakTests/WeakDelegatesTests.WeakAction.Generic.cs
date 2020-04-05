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
        public void Generic_WeakInstanceAction_AfterGarbageCollection_WithStrongReference_IsAlive<TIn>(TIn inputParameter)
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
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithoutReferences<TIn>());

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithoutReferences_AfterGarbageCollection_WithStrongReference_IsAlive<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithoutReferences<TIn>());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        public void Generic_WeakAnonymousActionWithoutReferences_AfterGarbageCollection_WithoutStrongReference_StillAlive<TIn>(TIn inputParameter)
        {
            var (reference, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithoutReferences<TIn>());

            reference.Dispose();
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
        public void Generic_WeakAnonymousActionWithInstanceReference_AfterGarbageCollection_WithStrongReference_IsAlive<TIn>(TIn inputParameter)
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
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter));

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        public void Generic_WeakAnonymousActionWithLocalReference_AfterGarbageCollection_WithStrongReference_IsAlive<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter));

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithLocalReference_WhenAlive_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter));

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakActionInputParameters))]
        public void Generic_WeakAnonymousActionWithLocalReference_WhenNotAlive_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter));

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
        public void Generic_WeakStaticAction_AfterGarbageCollection_IsAlive<TIn>(TIn inputParameter)
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
    }
}
