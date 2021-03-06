// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakActionTests
{
    public partial class WeakInstanceActionTests
    {
        [Fact]
        public void IsStatic_ReturnsFalse()
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            Assert.False(weakAction.IsStatic);
        }

        [Fact]
        public void IsAlive_WithTargetReference_AfterGarbageCollection_ReturnsTrue()
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_WithTargetReference_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction());

            weakAction.Execute();

            callCounter.Received(1).OnActionCalled();
        }

        [Fact]
        public void IsAlive_WithoutTargetReference_AfterGarbageCollection_ReturnsFalse()
        {
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_WithoutTargetReference_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }

        [Fact]
        public void IsAlive_WhenMarkedForDeletion_ReturnsFalse()
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction());

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_WhenMarkedForDeletion_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction());

            weakAction.MarkForDeletion();
            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }
    }
}
