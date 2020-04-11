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
        public void IsStatic_ForAnonymousActionWithInstanceReference_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForAnonymousActionWithInstanceReference_WithStrongReference_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForAnonymousActionWithInstanceReference_WithStrongReference_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForAnonymousActionWithInstanceReference_WithoutStrongReference_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForAnonymousActionWithInstanceReference_WithoutStrongReference_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousActionWithInstanceReference<TIn>());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }
    }
}
