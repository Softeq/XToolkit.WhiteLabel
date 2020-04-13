// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests.WeakFuncTests
{
    public partial class WeakStaticFuncTests
    {
        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetAlive_AfterGarbageCollection_ReturnsTrue<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetAlive_AfterGarbageCollection_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakFunc) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

                GC.Collect();

                weakFunc.Execute();

                callCounter.Received(1).OnFuncCalled<TOut>();
            }
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void IsAlive_WithCustomTargetDead_AfterGarbageCollection_ReturnsFalse<TOut>(TOut outputParameter)
        {
            var (customTarget, weakFunc) = WeakDelegateCreator
                .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakDelegatesTestsDataProvider.WeakFuncOutputParameters), MemberType = typeof(WeakDelegatesTestsDataProvider))]
        public void Execute_WithCustomTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakFunc) = WeakDelegateCreator
                    .CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

                customTarget.Dispose();
                GC.Collect();

                weakFunc.Execute();

                callCounter.DidNotReceive().OnFuncCalled<TOut>();
            }
        }
    }
}
