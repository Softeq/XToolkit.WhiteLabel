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
        public void IsAlive_ForStaticAction_WithCustomTargetAlive_WithAfterGarbageCollection_ReturnsTrue()
        {
            var (_, weakAction) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForStaticAction_WithCustomTargetAlive_WithAfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakAction) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

                GC.Collect();

                weakAction.Execute();

                callCounter.Received(1).OnActionCalled();
            }
        }

        [Fact]
        public void IsAlive_ForStaticAction_WithCustomTargetDead_AfterGarbageCollection_ReturnsFalse()
        {
            var (customTarget, weakAction) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_ForStaticAction_WithCustomTargetDead_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakAction) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction);

                customTarget.Dispose();
                GC.Collect();

                weakAction.Execute();

                callCounter.DidNotReceive().OnActionCalled();
            }
        }
    }
}
