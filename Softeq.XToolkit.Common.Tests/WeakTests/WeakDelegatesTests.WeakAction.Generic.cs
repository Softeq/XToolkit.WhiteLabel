using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.Helpers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        private static WeakAction<T> GetAnonimousGenericWeakAction<T>(Func<IGenericWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakAnonymousAction<T>();
        }

        private static WeakAction<T> GetPrivateGenericWeakAction<T>(Func<IGenericWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakPrivateAction<T>();
        }

        private static WeakAction<T> GetInternalGenericWeakAction<T>(Func<IGenericWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakInternalAction<T>();
        }

        private static WeakAction<T> GetPublicGenericWeakAction<T>(Func<IGenericWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakPublicAction<T>();
        }

        #region Internal
        [Fact]
        public void GenericWeakAction_InternalClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region InternalGeneric
        [Fact]
        public void GenericWeakAction_InternalGenericClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalGenericClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalGenericClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalGenericClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region InternalStatic
        [Fact]
        // Test shows why lambdas without catching execution context are evil. Roslyn compile anonymous
        // lambdas with no captured context into singletones, which will never get garbage collected
        public void GenericWeakAction_InternalStaticClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalStaticClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalStaticClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_InternalStaticClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }
        #endregion

        #region Public
        [Fact]
        public void GenericWeakAction_PublicClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region PublicGeneric
        [Fact]
        public void GenericWeakAction_PublicGenericClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicGenericClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicGenericClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicGenericClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region PublicStatic
        [Fact]
        // Test shows why lambdas without catching execution context are evil. Roslyn compile anonymous
        // lambdas with no captured context into singletones, which will never get garbage collected
        public void GenericWeakAction_PublicStaticClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicStaticClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicStaticClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_PublicStaticClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }
        #endregion

        #region Nested
        [Fact]
        public void GenericWeakAction_NestedClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_NestedClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_NestedClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_NestedClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region NestedGeneric
        [Fact]
        public void GenericWeakAction_NestedGenericClass_AnonymousMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetAnonimousGenericWeakAction<ITestType>(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunAnanimousAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_NestedGenericClass_PrivateMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPrivateGenericWeakAction<ITestType>(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPrivateAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_NestedGenericClass_InternalMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetInternalGenericWeakAction<ITestType>(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunInternalAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void GenericWeakAction_NestedGenericClass_PublicMethod_IsAlive()
        {
            var testParameter = Substitute.For<ITestType>();
            var weakAction = GetPublicGenericWeakAction<ITestType>(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute(testParameter);

            _callCounter.Received(1).RunPublicAction(testParameter);

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion
    }
}
