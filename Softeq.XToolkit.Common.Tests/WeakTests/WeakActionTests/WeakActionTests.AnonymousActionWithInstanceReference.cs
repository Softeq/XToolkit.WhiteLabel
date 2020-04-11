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
        public void IsStatic_ForAnonymousActionWithInstanceReference_ReturnsFalse()
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        public void IsAlive_ForAnonymousActionWithInstanceReference_WithStrongReference_AfterGarbageCollection_ReturnsTrue()
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForAnonymousActionWithInstanceReference_WithStrongReference_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void IsAlive_ForAnonymousActionWithInstanceReference_WithoutStrongReference_AfterGarbageCollection_ReturnsFalse()
        {
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForAnonymousActionWithInstanceReference_WithoutStrongReference_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }
    }
}
