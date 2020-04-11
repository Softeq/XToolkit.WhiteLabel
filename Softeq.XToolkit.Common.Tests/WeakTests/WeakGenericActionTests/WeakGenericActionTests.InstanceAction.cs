// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericActionTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public partial class WeakGenericActionTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ForInstanceAction_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            Assert.False(weakAction.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceAction_WithStrongReference_AfterGarbageCollection_ReturnsTrue<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceAction_WithStrongReference_AfterGarbageCollection_InvokesAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction<TIn>());

            weakAction.Execute(inputParameter);

            callCounter.Received(1).OnActionCalled(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceAction_WithoutStrongReference_AfterGarbageCollection_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceAction_WithoutStrongReference_AfterGarbageCollection_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction<TIn>());

            reference.Dispose();
            GC.Collect();

            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_ForInstanceDelegate_WhenMarkedForDeletion_ReturnsFalse<TIn>(TIn inputParameter)
        {
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceAction<TIn>());

            weakAction.MarkForDeletion();

            Assert.False(weakAction.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakActionInputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_ForInstanceDelegate_WhenMarkedForDeletion_DoesNotInvokeAction<TIn>(TIn inputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakAction) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceAction<TIn>());

            weakAction.MarkForDeletion();
            weakAction.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled(Arg.Any<TIn>());
        }
    }
}
