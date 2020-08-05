// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakFuncTests
{
    [SuppressMessage("ReSharper", "xUnit1026", Justification = "Generic parameters used just for test case generation")]
    public class WeakAnonymousFuncWithInstanceReferenceTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithTargetReference_AfterGarbageCollection_ReturnsTrue<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithTargetReference_AfterGarbageCollection_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            weakFunc.Execute();

            callCounter.Received(1).OnFuncCalled<TOut>();
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithoutTargetReference_AfterGarbageCollection_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var (reference, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithoutTargetReference_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakFunc) = WeakDelegateCreator.CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            reference.Dispose();
            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }
    }
}
