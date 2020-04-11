// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericActionTests
{
    public partial class WeakGenericActionTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ForAnonymousActionWithLocalReference_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter);

            Assert.False(weakAction.IsStatic);
        }

        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForAnonymousActionWithLocalReference_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForAnonymousActionWithLocalReference_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakAction = WeakDelegatesCallCounter.GetWeakAnonymousActionWithLocalReference<TIn>(callCounter);

            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }
    }
}
