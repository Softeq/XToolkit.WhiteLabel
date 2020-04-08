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
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_NotStatic<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_AfterGarbageCollection_WithStrongReference_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_WhenAlive_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            weakFunc.Execute(inputParameter);

            callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_AfterGarbageCollection_WithoutStrongReference_NotAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_WhenNotAlive_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakInstanceFunc<TIn, TOut>());

            reference.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        #endregion

        #region WeakAnonymousFuncWithoutReferences

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithoutReferences_NotStatic<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithoutReferences<TIn, TOut>();

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithoutReferences_AfterGarbageCollection_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithoutReferences<TIn, TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        #endregion

        #region WeakAnonymousFuncWithInstanceReference

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithInstanceReference_NotStatic<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TIn, TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithInstanceReference_AfterGarbageCollection_WithStrongReference_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TIn, TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithInstanceReference_WhenAlive_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TIn, TOut>());

            weakFunc.Execute(inputParameter);

            callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithInstanceReference_AfterGarbageCollection_WithoutStrongReference_NotAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TIn, TOut>());

            reference.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithInstanceReference_WhenNotAlive_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegatesCallCounter(callCounter),
                x => x.GetWeakAnonymousFuncWithInstanceReference<TIn, TOut>());

            reference.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        #endregion

        #region WeakAnonymousFuncWithLocalReference

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithLocalReference_NotStatic<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithLocalReference_AfterGarbageCollection_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithLocalReference_WhenAlive_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            weakFunc.Execute(inputParameter);

            callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithLocalReference_WhenNotAlive_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var weakFunc = WeakDelegatesCallCounter.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter);

            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        #endregion

        #region WeakStaticInstance

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_IsStatic<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

            Assert.True(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_AfterGarbageCollection_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_WhenExecuted_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>();

                weakFunc.Execute(inputParameter);

                callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
            }
        }

        #endregion

        #region WeakAnonymousFuncWithStaticReference

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithStaticReference_NotStatic<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TIn, TOut>();

            Assert.False(weakFunc.IsStatic);
        }

        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithStaticReference_AfterGarbageCollection_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TIn, TOut>();

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithStaticReference_WhenExecuted_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var weakFunc = StaticWeakDelegatesCallCounter.GetWeakAnonymousFuncWithStaticReference<TIn, TOut>();

                weakFunc.Execute(inputParameter);

                callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
            }
        }

        #endregion

        #region WeakInstanceFunc_WithCustomTarget

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetAlive_OriginalTargetAlive_AfterGarbageCollection_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_NotAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (customTarget, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetDead_OriginalTargetAlive_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, _, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(callCounter),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_NotAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetAlive_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            originalTarget.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_NotAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (customTarget, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakInstanceFunc_CustomTargetDead_OriginalTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (customTarget, originalTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(
                () => new WeakDelegatesCallCounter(),
                (x, y) => x.GetWeakInstanceFunc<TIn, TOut>(y));

            customTarget.Dispose();
            originalTarget.Dispose();
            GC.Collect();

            weakFunc.Execute(inputParameter);

            callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
        }

        #endregion

        #region WeakStaticFunc_WithCustomTarget

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_CustomTargetAlive_AfterGarbageCollection_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_CustomTargetAlive_AfterGarbageCollection_InvokesFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (_, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

                GC.Collect();

                weakFunc.Execute(inputParameter);

                callCounter.Received(1).OnFuncCalled<TIn, TOut>(inputParameter);
            }
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_CustomTargetDead_AfterGarbageCollection_NotAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (customTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

            customTarget.Dispose();
            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakStaticFunc_CustomTargetDead_AfterGarbageCollection_DoesNotInvokeFunc<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();

            using (StaticWeakDelegatesCallCounter.WithCallCounter(callCounter))
            {
                var (customTarget, weakFunc) = CreateWeakDelegateWithCustomTarget(StaticWeakDelegatesCallCounter.GetWeakStaticFunc<TIn, TOut>);

                customTarget.Dispose();
                GC.Collect();

                weakFunc.Execute(inputParameter);

                callCounter.DidNotReceive().OnFuncCalled<TIn, TOut>(Arg.Any<TIn>());
            }
        }

        #endregion
    }
}
