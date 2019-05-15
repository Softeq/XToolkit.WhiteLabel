// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Tests.Core.Common.Helpers;
using Softeq.XToolkit.Common;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common.WeakTests
{
    public class WeakActionNestedTest
    {
        private PublicNestedTestClass _itemPublic;
        private InternalNestedTestClass _itemInternal;
        private PrivateNestedTestClass _itemPrivate;
        private WeakReference _reference;
        private WeakAction _action;

        private void TestPublicNestedClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPublic = index.HasValue
                ? new PublicNestedTestClass(index.Value)
                : new PublicNestedTestClass();

            _reference = new WeakReference(_itemPublic);
            _action = _itemPublic.GetAction(weakActionTestCase);
        }

        private void TestInternalNestedClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemInternal = index.HasValue
                ? new InternalNestedTestClass(index.Value)
                : new InternalNestedTestClass();

            _reference = new WeakReference(_itemInternal);
            _action = _itemInternal.GetAction(weakActionTestCase);
        }

        private void TestPrivateNestedClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPrivate = index.HasValue
                ? new PrivateNestedTestClass(index.Value)
                : new PrivateNestedTestClass();

            _reference = new WeakReference(_itemPrivate);
            _action = _itemPrivate.GetAction(weakActionTestCase);
        }

        private void Reset()
        {
            _itemPublic = null;
            _itemInternal = null;
            _itemPrivate = null;
            _reference = null;
        }

        public class PublicNestedTestClass
        {
            public const string Expected = "Hello";
            public const string Public = "Public";
            public const string Internal = "Internal";
            public const string Private = "Private";
            public const string PublicStatic = "PublicStatic";
            public const string InternalStatic = "InternalStatic";
            public const string PrivateStatic = "PrivateStatic";
            private readonly int _index; // Just here to force instance methods

            public PublicNestedTestClass()
            {
            }

            public PublicNestedTestClass(int index)
            {
                _index = index;
            }

            public static string Result { get; private set; }

            private void DoStuffPrivately()
            {
                Result = Expected + Private + _index;
            }

            internal void DoStuffInternally()
            {
                Result = Expected + Internal + _index;
            }

            public void DoStuffPublically()
            {
                Result = Expected + Public + _index;
            }

            private static void DoStuffPrivatelyAndStatically()
            {
                Result = Expected + PrivateStatic;
            }

            public static void DoStuffPublicallyAndStatically()
            {
                Result = Expected + PublicStatic;
            }

            internal static void DoStuffInternallyAndStatically()
            {
                Result = Expected + InternalStatic;
            }

            public WeakAction GetAction(WeakActionTestCase testCase)
            {
                WeakAction action = null;

                switch (testCase)
                {
                    case WeakActionTestCase.PublicNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPublically);
                        break;
                    case WeakActionTestCase.InternalNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffInternally);
                        break;
                    case WeakActionTestCase.PrivateNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPrivately);
                        break;
                    case WeakActionTestCase.PublicStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPublicallyAndStatically);
                        break;
                    case WeakActionTestCase.InternalStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffInternallyAndStatically);
                        break;
                    case WeakActionTestCase.PrivateStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPrivatelyAndStatically);
                        break;
                    case WeakActionTestCase.AnonymousStaticMethod:
                        action = new WeakAction(
                            this,
                            () => Result = Expected);
                        break;
                    case WeakActionTestCase.AnonymousMethod:
                        action = new WeakAction(
                            this,
                            () => Result = Expected + _index);
                        break;
                }

                return action;
            }
        }

        internal class InternalNestedTestClass
        {
            public const string Expected = "Hello";
            public const string Public = "Public";
            public const string Internal = "Internal";
            public const string Private = "Private";
            public const string PublicStatic = "PublicStatic";
            public const string InternalStatic = "InternalStatic";
            public const string PrivateStatic = "PrivateStatic";
            private readonly int _index; // Just here to force instance methods

            public InternalNestedTestClass()
            {
            }

            public InternalNestedTestClass(int index)
            {
                _index = index;
            }

            public static string Result { get; private set; }

            private void DoStuffPrivately()
            {
                Result = Expected + Private + _index;
            }

            internal void DoStuffInternally()
            {
                Result = Expected + Internal + _index;
            }

            public void DoStuffPublically()
            {
                Result = Expected + Public + _index;
            }

            private static void DoStuffPrivatelyAndStatically()
            {
                Result = Expected + PrivateStatic;
            }

            internal static void DoStuffInternallyAndStatically()
            {
                Result = Expected + InternalStatic;
            }

            public static void DoStuffPublicallyAndStatically()
            {
                Result = Expected + PublicStatic;
            }

            public WeakAction GetAction(WeakActionTestCase testCase)
            {
                WeakAction action = null;

                switch (testCase)
                {
                    case WeakActionTestCase.PublicNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPublically);
                        break;
                    case WeakActionTestCase.InternalNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffInternally);
                        break;
                    case WeakActionTestCase.PrivateNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPrivately);
                        break;
                    case WeakActionTestCase.PublicStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPublicallyAndStatically);
                        break;
                    case WeakActionTestCase.InternalStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffInternallyAndStatically);
                        break;
                    case WeakActionTestCase.PrivateStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPrivatelyAndStatically);
                        break;
                    case WeakActionTestCase.AnonymousStaticMethod:
                        action = new WeakAction(
                            this,
                            () => Result = Expected);
                        break;
                    case WeakActionTestCase.AnonymousMethod:
                        action = new WeakAction(
                            this,
                            () => Result = Expected + _index);
                        break;
                }

                return action;
            }
        }

        private class PrivateNestedTestClass
        {
            public const string Expected = "Hello";
            public const string Public = "Public";
            public const string Internal = "Internal";
            public const string Private = "Private";
            public const string PublicStatic = "PublicStatic";
            public const string InternalStatic = "InternalStatic";
            public const string PrivateStatic = "PrivateStatic";
            private readonly int _index; // Just here to force instance methods

            public PrivateNestedTestClass()
            {
            }

            public PrivateNestedTestClass(int index)
            {
                _index = index;
            }

            public static string Result { get; private set; }

            private void DoStuffPrivately()
            {
                Result = Expected + Private + _index;
            }

            internal void DoStuffInternally()
            {
                Result = Expected + Internal + _index;
            }

            public void DoStuffPublically()
            {
                Result = Expected + Public + _index;
            }

            private static void DoStuffPrivatelyAndStatically()
            {
                Result = Expected + PrivateStatic;
            }

            internal static void DoStuffInternallyAndStatically()
            {
                Result = Expected + InternalStatic;
            }

            public static void DoStuffPublicallyAndStatically()
            {
                Result = Expected + PublicStatic;
            }

            public WeakAction GetAction(WeakActionTestCase testCase)
            {
                WeakAction action = null;

                switch (testCase)
                {
                    case WeakActionTestCase.PublicNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPublically);
                        break;
                    case WeakActionTestCase.InternalNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffInternally);
                        break;
                    case WeakActionTestCase.PrivateNamedMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPrivately);
                        break;
                    case WeakActionTestCase.PublicStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPublicallyAndStatically);
                        break;
                    case WeakActionTestCase.InternalStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffInternallyAndStatically);
                        break;
                    case WeakActionTestCase.PrivateStaticMethod:
                        action = new WeakAction(
                            this,
                            DoStuffPrivatelyAndStatically);
                        break;
                    case WeakActionTestCase.AnonymousStaticMethod:
                        action = new WeakAction(
                            this,
                            () => Result = Expected);
                        break;
                    case WeakActionTestCase.AnonymousMethod:
                        action = new WeakAction(
                            this,
                            () => Result = Expected + _index);
                        break;
                }

                return action;
            }
        }

        [Fact]
        public void TestInternalNestedClassAnonymousMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + index,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassAnonymousStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + InternalNestedTestClass.Internal + index,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassInternalStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + InternalNestedTestClass.InternalStatic,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + InternalNestedTestClass.Private + index,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassPrivateStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + InternalNestedTestClass.PrivateStatic,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestInternalNestedClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + InternalNestedTestClass.Public + index,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalNestedClassPublicStaticMethod()
        {
            Reset();

            TestInternalNestedClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                InternalNestedTestClass.Expected + InternalNestedTestClass.PublicStatic,
                InternalNestedTestClass.Result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassAnonymousMethod()
        {
            Reset();

            const int index = 99;

            TestPrivateNestedClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + index,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassAnonymousStaticMethod()
        {
            Reset();

            TestPrivateNestedClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPrivateNestedClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + PrivateNestedTestClass.Internal + index,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassInternalStaticMethod()
        {
            Reset();

            TestPrivateNestedClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + PrivateNestedTestClass.InternalStatic,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPrivateNestedClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + PrivateNestedTestClass.Private + index,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassPrivateStaticMethod()
        {
            Reset();

            TestPrivateNestedClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + PrivateNestedTestClass.PrivateStatic,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPrivateNestedClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + PrivateNestedTestClass.Public + index,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateNestedClassPublicStaticMethod()
        {
            Reset();

            TestPrivateNestedClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PrivateNestedTestClass.Expected + PrivateNestedTestClass.PublicStatic,
                PrivateNestedTestClass.Result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassAnonymousMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + index,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassAnonymousStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassInternalNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + PublicNestedTestClass.Internal + index,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassInternalStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + PublicNestedTestClass.InternalStatic,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassPrivateNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + PublicNestedTestClass.Private + index,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassPrivateStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + PublicNestedTestClass.PrivateStatic,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;

            TestPublicNestedClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + PublicNestedTestClass.Public + index,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicNestedClassPublicStaticMethod()
        {
            Reset();

            TestPublicNestedClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            _action.Execute();

            Assert.Equal(
                PublicNestedTestClass.Expected + PublicNestedTestClass.PublicStatic,
                PublicNestedTestClass.Result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }
    }
}