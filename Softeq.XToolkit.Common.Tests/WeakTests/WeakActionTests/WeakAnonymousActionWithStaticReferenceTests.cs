// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakActionTests
{
    [Collection(nameof(StaticWeakDelegatesCallCounter))]
    public class WeakAnonymousActionWithStaticReferenceTests
    {
        [Fact]
        public void IsStatic_ReturnsFalse()
        {
            var weakAction = StaticWeakDelegatesCallCounter
                .GetWeakAnonymousActionWithStaticReference();

            Assert.False(weakAction.IsStatic);
        }

        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        [Fact]
        public void IsAlive_AfterGarbageCollection_ReturnsTrue()
        {
            var weakAction = StaticWeakDelegatesCallCounter
                .GetWeakAnonymousActionWithStaticReference();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_AfterGarbageCollection_InvokesAction()
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter
                    .GetWeakAnonymousActionWithStaticReference();

                GC.Collect();
                weakAction.Execute();

                callCounter.Received(1).OnActionCalled();
            }
        }
    }
}
