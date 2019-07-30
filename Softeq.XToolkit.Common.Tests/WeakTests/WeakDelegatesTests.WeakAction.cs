using System;
using NSubstitute;
using Softeq.XToolkit.Common.Tests.Helpers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.WeakTests
{
    public partial class WeakDelegatesTests
    {
        private static WeakAction GetAnonimousWeakAction(Func<IWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakAnonymousAction();
        }

        private static WeakAction GetPrivateWeakAction(Func<IWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakPrivateAction();
        }

        private static WeakAction GetInternalWeakAction(Func<IWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakInternalAction();
        }

        private static WeakAction GetPublicWeakAction(Func<IWeakActionProvider> weakActionProvider)
        {
            return weakActionProvider.Invoke().GetWeakPublicAction();
        }

        #region Internal
        [Fact]
        public void WeakAction_InternalClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreateInternalWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region InternalGeneric
        [Fact]
        public void WeakAction_InternalGenericClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalGenericClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalGenericClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalGenericClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreateInternalGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region InternalStatic
        [Fact]
        // Test shows why lambdas without catching execution context are evil. Roslyn compile anonymous
        // lambdas with no captured context into singletones, which will never get garbage collected
        public void WeakAction_InternalStaticClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalStaticClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalStaticClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalStaticClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreateInternalStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }
        #endregion

        #region Public
        [Fact]
        public void WeakAction_PublicClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreatePublicWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region PublicGeneric
        [Fact]
        public void WeakAction_PublicGenericClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicGenericClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicGenericClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicGenericClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreatePublicGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region PublicStatic
        [Fact]
        // Test shows why lambdas without catching execution context are evil. Roslyn compile anonymous
        // lambdas with no captured context into singletones, which will never get garbage collected
        public void WeakAction_PublicStaticClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);  // since C# 6 lambdas are never static

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicStaticClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicStaticClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_PublicStaticClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreatePublicStaticWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.True(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.True(weakAction.IsAlive);
        }
        #endregion

        #region Nested
        [Fact]
        public void WeakAction_NestedClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_NestedClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_NestedClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_NestedClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreateNestedWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion

        #region NestedGeneric
        [Fact]
        public void WeakAction_NestedGenericClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanymousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_NestedGenericClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPrivateAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_NestedGenericClass_InternalMethod_IsAlive()
        {
            var weakAction = GetInternalWeakAction(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunInternalAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_NestedGenericClass_PublicMethod_IsAlive()
        {
            var weakAction = GetPublicWeakAction(CreateNestedGenericWeakDelegateProvider);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
        #endregion
    }
}
