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
    [Collection(nameof(StaticWeakDelegatesCallCounter))]
    public partial class WeakStaticFuncTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsStatic_ReturnsTrue<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

            Assert.True(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_AfterGarbageCollection_ReturnsTrue<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_AfterGarbageCollection_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

                GC.Collect();
                weakFunc.Execute();

                callCounter.Received(1).OnFuncCalled<TOut>();
            }
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WhenMarkedForDeletion_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

            weakFunc.MarkForDeletion();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WhenMarkedForDeletion_DoesNotInvokeAction<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

                weakFunc.MarkForDeletion();
                weakFunc.Execute();

                callCounter.DidNotReceive().OnActionCalled();
            }
        }
    }
}
