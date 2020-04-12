// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericActionTests
{
    public partial class WeakGenericStaticActionTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetAlive_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetAlive_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakAction) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

                GC.Collect();

                weakAction.Execute(inputParameter);

                callCounter.Received(1).OnActionCalled(inputParameter);
            }
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetDead_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (customTarget, weakAction) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetDead_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakAction) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>);

                customTarget.Dispose();
                GC.Collect();

                weakAction.Execute(inputParameter);

                callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
            }
        }
    }
}
