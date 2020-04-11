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
        public void IsStatic_ForStaticAction_ReturnsTrue()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

            Assert.True(weakAction.IsStatic);
        }

        [Fact]
        public void IsAlive_ForStaticAction_AfterGarbageCollection_ReturnsTrue()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForStaticAction_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

                GC.Collect();
                weakAction.Execute();

                callCounter.Received(1).OnActionCalled();
            }
        }

        [Fact]
        public void IsAlive_ForStaticDelegate_WhenMarkedForDeletion_ReturnsFalse()
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction();

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForStaticDelegate_WhenMarkedForDeletion_DoesNotInvokeAction()
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
    }
}
