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
        public void Generic_WeakInstanceFunc_AfterGarbageCollection_WithStrongReference_IsAlive<TIn, TOut>(
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
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithoutReferences<TIn, TOut>());

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        public void Generic_WeakAnonymousFuncWithoutReferences_AfterGarbageCollection_WithStrongReference_IsAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithoutReferences<TIn, TOut>());

            GC.Collect();

            Assert.True(weakFunc.IsAlive);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        // This test shows that even if lambdas has no references - compiler creates singleton for each one of them!
        public void Generic_WeakAnonymousFuncWithoutReferences_AfterGarbageCollection_WithoutStrongReference_StillAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var (reference, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithoutReferences<TIn, TOut>());

            reference.Dispose();
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
        public void Generic_WeakAnonymousFuncWithInstanceReference_AfterGarbageCollection_WithStrongReference_IsAlive<TIn, TOut>(
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
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter));

            Assert.False(weakFunc.IsStatic);
        }

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        // This test shows why WeakDelegate for lambdas with local variable references doesn't work:
        // compiler creates instance of inner class, that could be garbage collected as soon as method ends
        public void Generic_WeakAnonymousFuncWithLocalReference_AfterGarbageCollection_WithStrongReference_IsAlive<TIn, TOut>(
            TIn inputParameter,
            TOut outputParameter)
        {
            var callCounter = Substitute.For<ICallCounter>();
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter));

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
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter));

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
            var (_, weakFunc) = CreateWeakDelegate(
                () => new WeakDelegateInstanceTestClass(),
                x => WeakDelegateInstanceTestClass.GetWeakAnonymousFuncWithLocalReference<TIn, TOut>(callCounter));

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
        public void Generic_WeakStaticFunc_AfterGarbageCollection_IsAlive<TIn, TOut>(
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

        [Theory]
        [MemberData(nameof(WeakFuncInputOutputParameters))]
        // This test shows that even if lambdas has only static references - compiler creates singleton for each one of them!
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
    }
}
