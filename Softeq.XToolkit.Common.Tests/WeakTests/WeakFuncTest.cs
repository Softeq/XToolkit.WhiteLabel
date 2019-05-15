// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Tests.Core.Common.Helpers;
using Softeq.XToolkit.Common;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common.WeakTests
{
    public class WeakFuncTest
    {
        private WeakFunc<string> _action;
        private CommonTestClass _common;
        private InternalTestClass _itemInternal;
        private PublicTestClass _itemPublic;
        private string _local;
        private WeakReference _reference;

        private void TestPublicClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPublic = index.HasValue
                ? new PublicTestClass(index.Value)
                : new PublicTestClass();

            _reference = new WeakReference(_itemPublic);
            _action = _itemPublic.GetFunc(weakActionTestCase);
        }

        private void TestInternalClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemInternal = index.HasValue
                ? new InternalTestClass(index.Value)
                : new InternalTestClass();

            _reference = new WeakReference(_itemInternal);
            _action = _itemInternal.GetFunc(weakActionTestCase);
        }

        private void TestCommonSetup()
        {
            _common = new CommonTestClass();
            _reference = new WeakReference(_common);
            _action = new WeakFunc<string>(_common, DoStuffStaticWithResult);
        }

        public static string DoStuffStaticWithResult()
        {
            return DateTime.Now.ToString();
        }

        public string DoStuffWithResult()
        {
            _local = DateTime.Now.ToString();
            return _local;
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

            TestInternalClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + index,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + index,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassAnonymousStaticMethod()
        {
            Reset();

            TestInternalClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Internal + index,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Internal + index,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalStaticMethod()
        {
            Reset();

            TestInternalClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.InternalStatic,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.InternalStatic,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Private + index,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Private + index,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateStaticMethod()
        {
            Reset();

            TestInternalClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.PrivateStatic,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.PrivateStatic,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Public + index,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.Public + index,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicStaticMethod()
        {
            Reset();

            TestInternalClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.PublicStatic,
                InternalTestClass.Result);
            Assert.Equal(
                InternalTestClass.Expected + InternalTestClass.PublicStatic,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestNonStaticMethodWithNullTarget()
        {
            Reset();
            var func = new WeakFunc<string>(null, DoStuffWithResult);
            Assert.False(func.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousMethod()
        {
            Reset();

            const int index = 99;

            TestPublicClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + index,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + index,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousStaticMethod()
        {
            Reset();

            TestPublicClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Internal + index,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Internal + index,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalStaticMethod()
        {
            Reset();

            TestPublicClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.InternalStatic,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.InternalStatic,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Private + index,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Private + index,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateStaticMethod()
        {
            Reset();

            TestPublicClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.PrivateStatic,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.PrivateStatic,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Public + index,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.Public + index,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicStaticMethod()
        {
            Reset();

            TestPublicClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute();

            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.PublicStatic,
                PublicTestClass.Result);
            Assert.Equal(
                PublicTestClass.Expected + PublicTestClass.PublicStatic,
                result);

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
            var func = new WeakFunc<string>(null, DoStuffStaticWithResult);
            Assert.True(func.IsAlive);
        }
    }
}