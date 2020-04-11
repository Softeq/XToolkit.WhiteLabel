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
        public void IsStatic_ForAnonymousActionWithStaticReference_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference<TIn>();

            Assert.False(weakAction.IsStatic);
        }

        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForAnonymousActionWithStaticReference_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference<TIn>();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForAnonymousActionWithStaticReference_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakAnonymousActionWithStaticReference<TIn>();

                GC.Collect();
                weakAction.Execute(inputParameter);

                callCounter.Received(1).OnActionCalled(inputParameter);
            }
        }
    }
}
