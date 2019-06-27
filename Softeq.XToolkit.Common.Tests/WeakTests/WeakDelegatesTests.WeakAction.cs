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

        [Fact]
        public void WeakAction_InternalClass_AnonymousMethod_IsAlive()
        {
            var weakAction = GetAnonimousWeakAction(CreateInternalClass);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunAnanimousAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }

        [Fact]
        public void WeakAction_InternalClass_PrivateMethod_IsAlive()
        {
            var weakAction = GetPrivateWeakAction(CreateInternalClass);

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
            var weakAction = GetInternalWeakAction(CreateInternalClass);

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
            var weakAction = GetPublicWeakAction(CreateInternalClass);

            Assert.True(weakAction.IsAlive);
            Assert.False(weakAction.IsStatic);

            weakAction.Execute();

            _callCounter.Received(1).RunPublicAction();

            GC.Collect();

            Assert.False(weakAction.IsAlive);
        }
    }
}
