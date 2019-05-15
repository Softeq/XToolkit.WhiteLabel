// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Tests.Core.Common.Helpers;
using Softeq.XToolkit.Common;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common.WeakTests
{
    public class WeakActionGenericTest
    {
        private PublicTestClass<string> _itemPublic;
        private InternalTestClass<string> _itemInternal;
        private CommonTestClass _common;
        private WeakReference _reference;
        private WeakAction<string> _action;
        private string _local;

        private void TestPublicClassPublicSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPublic = index.HasValue
                ? new PublicTestClass<string>(index.Value)
                : new PublicTestClass<string>();

            _reference = new WeakReference(_itemPublic);
            _action = _itemPublic.GetAction(weakActionTestCase);
        }

        private void TestInternalClassPublicSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemInternal = index.HasValue
                ? new InternalTestClass<string>(index.Value)
                : new InternalTestClass<string>();

            _reference = new WeakReference(_itemInternal);
            _action = _itemInternal.GetAction(weakActionTestCase);
        }

        private void TestCommonSetup()
        {
            _common = new CommonTestClass();
            _reference = new WeakReference(_common);
            _action = new WeakAction<string>(_common, DoStuffStatic);
        }

        public static void DoStuffStatic(string p)
        {
        }

        public void DoStuff(string p)
        {
            _local = p;
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
            const string parameter = "My parameter";

            TestInternalClassPublicSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + index + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassAnonymousStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassPublicSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalNamedMethod()
        {
            Reset();

            const string parameter = "My parameter";
            const int index = 99;

            TestInternalClassPublicSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + InternalTestClass<string>.Internal + index + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassPublicSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + InternalTestClass<string>.InternalStatic + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateNamedMethod()
        {
            Reset();

            const string parameter = "My parameter";
            const int index = 99;

            TestInternalClassPublicSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + InternalTestClass<string>.Private + index + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassPublicSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + InternalTestClass<string>.PrivateStatic + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestInternalClassPublicSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + InternalTestClass<string>.Public + index + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassPublicSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                InternalTestClass<string>.Expected + InternalTestClass<string>.PublicStatic + parameter,
                InternalTestClass<string>.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestNonStaticMethodWithNullTarget()
        {
            Reset();
            var action = new WeakAction<string>(null, DoStuff);
            Assert.False(action.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestPublicClassPublicSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + index + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassPublicSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalNamedMethod()
        {
            Reset();

            const string parameter = "My parameter";
            const int index = 99;

            TestPublicClassPublicSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + PublicTestClass<string>.Internal + index + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassPublicSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + PublicTestClass<string>.InternalStatic + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateNamedMethod()
        {
            Reset();

            const string parameter = "My parameter";
            const int index = 99;

            TestPublicClassPublicSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + PublicTestClass<string>.Private + index + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassPublicSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + PublicTestClass<string>.PrivateStatic + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestPublicClassPublicSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + PublicTestClass<string>.Public + index + parameter,
                PublicTestClass<string>.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassPublicSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute(parameter);

            Assert.Equal(
                PublicTestClass<string>.Expected + PublicTestClass<string>.PublicStatic + parameter,
                PublicTestClass<string>.Result);

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
            var action = new WeakAction<string>(null, DoStuffStatic);
            Assert.True(action.IsAlive);
        }
    }
}