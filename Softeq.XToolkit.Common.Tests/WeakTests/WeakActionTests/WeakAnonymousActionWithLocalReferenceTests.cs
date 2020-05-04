// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakActionTests
{
    public class WeakAnonymousActionWithLocalReferenceTests
    {
        [Fact]
        public void IsStatic_ReturnsFalse()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference(callCounter);

            Assert.False(weakAction.IsStatic);
        }

        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        [Fact]
        public void IsAlive_AfterGarbageCollection_ReturnsFalse()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference(callCounter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void Execute_AfterGarbageCollection_DoesNotInvokeAction()
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference(callCounter);

            GC.Collect();

            weakAction.Execute();

            callCounter.DidNotReceive().OnActionCalled();
        }
    }
}
