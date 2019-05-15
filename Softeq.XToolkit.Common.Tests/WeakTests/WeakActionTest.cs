// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Tests.Core.Common.Helpers;
using Softeq.XToolkit.Common;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common.WeakTests
{
    public class WeakActionTest
    {
        private WeakAction _action;
        private CommonTestClass _common;
        private InternalTestClass _itemInternal;
        private PublicTestClass _itemPublic;
        private string _local;
        private WeakReference _reference;

        private void TestPublicNestedClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPublic = index.HasValue
                ? new PublicTestClass(index.Value)
                : new PublicTestClass();

            _reference = new WeakReference(_itemPublic);
            _action = _itemPublic.GetAction(weakActionTestCase);
        }

        private void TestInternalNestedClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemInternal = index.HasValue
                ? new InternalTestClass(index.Value)
                : new InternalTestClass();

            _reference = new WeakReference(_itemInternal);
            _action = _itemInternal.GetAction(weakActionTestCase);
        }

        private void TestCommonSetup()
        {
            _common = new CommonTestClass();
            _reference = new WeakReference(_common);
            _action = new WeakAction(_common, DoStuffStatic);
        }

        public static void DoStuffStatic()
        {
        }

        public void DoStuff()
        {
            _local = DateTime.Now.ToString();
        }

        private void Reset()
        {
            _itemPublic = null;
            _itemInternal = null;
            _reference = null;
        }

        [Fact]
        public void TestInternalClassAnonymousMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + index,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassAnonymousStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Internal + index,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.InternalStatic,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Private + index,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.PrivateStatic,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Public + index,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.PublicStatic,
                InternalTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestNonStaticMethodWithNullTarget()
        {
            Reset();
            var action = new WeakAction(null, DoStuff);
            Assert.False(action.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + index,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Internal + index,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.InternalStatic,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Private + index,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.PrivateStatic,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Public + index,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.PublicStatic,
                PublicTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestStaticMethodWithNonNullTarget()
        {
            Reset();

            TestCommonSetup();

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _common = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
            Assert.False(_action.IsAlive);
        }

        [Fact]
        public void TestStaticMethodWithNullTarget()
        {
            Reset();
            var action = new WeakAction(null, DoStuffStatic);
            Assert.True(action.IsAlive);
        }
    }
}