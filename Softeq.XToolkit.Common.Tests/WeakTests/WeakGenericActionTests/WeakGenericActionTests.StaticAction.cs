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
        public void IsStatic_ForStaticAction_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

            Assert.True(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForStaticAction_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForStaticAction_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

                GC.Collect();
                weakAction.Execute(inputParameter);

                callCounter.Received(1).OnActionCalled(inputParameter);
            }
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForStaticDelegate_WhenMarkedForDeletion_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForStaticDelegate_WhenMarkedForDeletion_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakAction = StaticWeakDelegatesCallCounter.GetWeakStaticAction<TIn>();

                weakAction.MarkForDeletion();
                weakAction.Execute(inputParameter);

                callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
            }
        }
    }
}
