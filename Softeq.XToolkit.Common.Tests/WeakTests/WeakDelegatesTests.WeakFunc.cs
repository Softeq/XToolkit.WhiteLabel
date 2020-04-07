// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        #region WeakInstanceFunc

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_NotStatic<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_AfterGarbageCollection_WithStrongReference_StillAlive<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_WhenAlive_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TOut>());

            weakFunc.Execute();

            callCounter.Received(1).OnFuncCalled<TOut>();
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_AfterGarbageCollection_WithoutStrongReference_NotAlive<TOut>(TOut outputParameter)
        {
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TOut>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_WhenNotAlive_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TOut>());

            reference.Dispose();
            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }

        #endregion

        #region WeakAnonymousFuncWithoutReferences

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithoutReferences_NotStatic<TOut>(TOut outputParameter)
        {
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithoutReferences<TOut>();

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        public void WeakAnonymousFuncWithoutReferences_AfterGarbageCollection_StillAlive<TOut>(TOut outputParameter)
        {
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithoutReferences<TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        #endregion

        #region WeakAnonymousFuncWithInstanceReference

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithInstanceReference_NotStatic<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithInstanceReference_AfterGarbageCollection_WithStrongReference_StillAlive<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithInstanceReference_WhenAlive_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            weakFunc.Execute();

            callCounter.Received(1).OnFuncCalled<TOut>();
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithInstanceReference_AfterGarbageCollection_WithoutStrongReference_NotAlive<TOut>(TOut outputParameter)
        {
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithInstanceReference_WhenNotAlive_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TOut>());

            reference.Dispose();
            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }

        #endregion

        #region WeakAnonymousFuncWithLocalReference

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithLocalReference_NotStatic<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TOut>(callCounter);

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        public void WeakAnonymousFuncWithLocalReference_AfterGarbageCollection_StillAlive<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TOut>(callCounter);

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithLocalReference_WhenNotAlive_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TOut>(callCounter);

            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }

        #endregion

        #region WeakStaticInstance

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_IsStatic<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

            Assert.True(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_AfterGarbageCollection_StillAlive<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_WhenExecuted_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>();

                weakFunc.Execute();

                callCounter.Received(1).OnFuncCalled<TOut>();
            }
        }

        #endregion

        #region WeakAnonymousFuncWithStaticReference

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithStaticReference_NotStatic<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TOut>();

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        public void WeakAnonymousFuncWithStaticReference_AfterGarbageCollection_StillAlive<TOut>(TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakAnonymousFuncWithStaticReference_WhenExecuted_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TOut>();

                weakFunc.Execute();

                callCounter.Received(1).OnFuncCalled<TOut>();
            }
        }

        #endregion

        #region WeakInstanceFunc_WithCustomTarget

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_StillAlive<TOut>(TOut outputParameter)
        {
            var (_, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x,y) => x.GetWeakInstanceFunc<TOut>(y));

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceFunc<TOut>(y));

            GC.Collect();

            weakFunc.Execute();

            callCounter.Received(1).OnFuncCalled<TOut>();
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_NotAlive<TOut>(TOut outputParameter)
        {
            var (customTarget, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TOut>(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceFunc<TOut>(y));

            customTarget.Dispose();
            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_NotAlive<TOut>(TOut outputParameter)
        {
            var (_, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TOut>(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TOut>(y));

            originalTarget.Dispose();
            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_NotAlive<TOut>(TOut outputParameter)
        {
            var (customTarget, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x,y) => x.GetWeakInstanceFunc<TOut>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakInstanceFunc_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x,y) => x.GetWeakInstanceFunc<TOut>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakFunc.Execute();

            callCounter.DidNotReceive().OnFuncCalled<TOut>();
        }

        #endregion

        #region WeakStaticFunc_WithCustomTarget

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_CustomTargetAlive_AfterGarbageCollection_StillAlive<TOut>(TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_CustomTargetAlive_AfterGarbageCollection_InvokesFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

                GC.Collect();

                weakFunc.Execute();

                callCounter.Received(1).OnFuncCalled<TOut>();
            }
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_CustomTargetDead_AfterGarbageCollection_NotAlive<TOut>(TOut outputParameter)
        {
            var (customTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncOutputParameters))]
        public void WeakStaticFunc_CustomTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TOut>(TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TOut>);

                customTarget.Dispose();
                GC.Collect();

                weakFunc.Execute();

                callCounter.DidNotReceive().OnFuncCalled<TOut>();
            }
        }

        #endregion
    }
}
