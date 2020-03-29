using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.Helpers;
using Softeq.XToolkit.Common.Tests.WeakTests.Utils;
using Softeq.XToolkit.Common.Weak;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        private static WeakFunc<T> GetAnonimousWeakFunc<T>(Func<IWeakFuncProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakAnonymousFunc<T>();
        }

        private static WeakFunc<T> GetPrivateWeakFunc<T>(Func<IWeakFuncProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakPrivateFunc<T>();
        }

        private static WeakFunc<T> GetInternalWeakFunc<T>(Func<IWeakFuncProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakInternalFunc<T>();
        }

        private static WeakFunc<T> GetPublicWeakFunc<T>(Func<IWeakFuncProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakPublicFunc<T>();
        }

        #region Internal
        [Fact]
        public void WeakFunc_InternalClass_AnonymousMethod_IsAlive()
        {
            var testResult = Substitute.For<ITestType>();
            var weakFunc = GetAnonimousWeakFunc<ITestType>(CreateInternalWeakDelegateProvider);

            _callCounter.RunAnanimousFunc<ITestType>().Returns(testResult);

            Assert.True(weakFunc.IsAlive);
            Assert.False(weakFunc.IsStatic);

            var result = weakFunc.Execute();

            Assert.True(ReferenceEquals(result, testResult));
            _callCounter.Received(1).RunAnanimousFunc<ITestType>();

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Fact]
        public void WeakFunc_InternalClass_PrivateMethod_IsAlive()
        {
            var testResult = Substitute.For<ITestType>();
            var weakFunc = GetPrivateWeakFunc<ITestType>(CreateInternalWeakDelegateProvider);

            _callCounter.RunPrivateFunc<ITestType>().Returns(testResult);

            Assert.True(weakFunc.IsAlive);
            Assert.False(weakFunc.IsStatic);

            var result = weakFunc.Execute();

            Assert.True(ReferenceEquals(result, testResult));
            _callCounter.Received(1).RunPrivateFunc<ITestType>();

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Fact]
        public void WeakFunc_InternalClass_InternalMethod_IsAlive()
        {
            var testResult = Substitute.For<ITestType>();
            var weakFunc = GetInternalWeakFunc<ITestType>(CreateInternalWeakDelegateProvider);

            _callCounter.RunInternalFunc<ITestType>().Returns(testResult);

            Assert.True(weakFunc.IsAlive);
            Assert.False(weakFunc.IsStatic);

            var result = weakFunc.Execute();

            Assert.True(ReferenceEquals(result, testResult));
            _callCounter.Received(1).RunInternalFunc<ITestType>();

            GC.Collect();

            Assert.False(weakFunc.IsAlive);
        }

        [Fact]
        public void WeakFunc_InternalClass_PublicMethod_IsAlive()
        {
            var testResult = Substitute.For<ITestType>();
            var weakAction = GetPublicWeakFunc<ITestType>(CreateInternalWeakDelegateProvider);

            _callCounter.RunPublicFunc<ITestType>().Returns(testResult);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            var result = weakAction.Execute();

            Assert.True(ReferenceEquals(result, testResult));
            _callCounter.Received(1).RunPublicFunc<ITestType>();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion
    }
}
