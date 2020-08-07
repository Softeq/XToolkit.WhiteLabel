// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakGenericFuncTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public partial class WeakGenericInstanceFuncTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithTargetReference_AfterGarbageCollection_ReturnsTrue<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithTargetReference_AfterGarbageCollection_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            weakFunc.Execute(inputParameter);

            callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithoutTargetReference_AfterGarbageCollection_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (reference, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithoutTargetReference_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            reference.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WhenMarkedForDeletion_ReturnsFalse<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            weakFunc.MarkForDeletion();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncInputOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WhenMarkedForDeletion_DoesNotInvokeAction<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            weakFunc.MarkForDeletion();
            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnActionCalled();
        }
    }
}
