// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Tests.Core.Common.Helpers;
using Softeq.XToolkit.Common;
using Xunit;

namespace Softeq.XToolkit.Tests.Core.Common.WeakTests
{
    public class WeakFuncGenericNestedTest
    {
        private WeakFunc<string, string> _action;
        private InternalNestedTestClass<string> _itemInternal;
        private PrivateNestedTestClass<string> _itemPrivate;
        private PublicNestedTestClass<string> _itemPublic;
        private WeakReference _reference;

        private void TestPublicClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPublic = index.HasValue
                ? new PublicNestedTestClass<string>(index.Value)
                : new PublicNestedTestClass<string>();

            _reference = new WeakReference(_itemPublic);
            _action = _itemPublic.GetFunc(weakActionTestCase);
        }

        private void TestInternalClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemInternal = index.HasValue
                ? new InternalNestedTestClass<string>(index.Value)
                : new InternalNestedTestClass<string>();

            _reference = new WeakReference(_itemInternal);
            _action = _itemInternal.GetFunc(weakActionTestCase);
        }

        private void TestPrivateClassSetup(WeakActionTestCase weakActionTestCase, int? index = null)
        {
            _itemPrivate = index.HasValue
                ? new PrivateNestedTestClass<string>(index.Value)
                : new PrivateNestedTestClass<string>();

            _reference = new WeakReference(_itemPrivate);
            _action = _itemPrivate.GetFunc(weakActionTestCase);
        }

        private void Reset()
        {
            _itemPublic = null;
            _itemInternal = null;
            _itemPrivate = null;
            _reference = null;
        }

        public class PublicNestedTestClass<T>
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

            private void DoStuffPrivately(T parameter)
            {
                Result = Expected + Private + _index + parameter;
            }

            internal void DoStuffInternally(T parameter)
            {
                Result = Expected + Internal + _index + parameter;
            }

            public void DoStuffPublically(T parameter)
            {
                Result = Expected + Public + _index + parameter;
            }

            private static void DoStuffPrivatelyAndStatically(T parameter)
            {
                Result = Expected + PrivateStatic + parameter;
            }

            public static void DoStuffPublicallyAndStatically(T parameter)
            {
                Result = Expected + PublicStatic + parameter;
            }

            internal static void DoStuffInternallyAndStatically(T parameter)
            {
                Result = Expected + InternalStatic + parameter;
            }

            private string DoStuffPrivatelyWithResult(T parameter)
            {
                Result = Expected + Private + _index + parameter;
                return Result;
            }

            internal string DoStuffInternallyWithResult(T parameter)
            {
                Result = Expected + Internal + _index + parameter;
                return Result;
            }

            public string DoStuffPublicallyWithResult(T parameter)
            {
                Result = Expected + Public + _index + parameter;
                return Result;
            }

            private static string DoStuffPrivatelyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + PrivateStatic + parameter;
                return Result;
            }

            public static string DoStuffPublicallyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + PublicStatic + parameter;
                return Result;
            }

            internal static string DoStuffInternallyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + InternalStatic + parameter;
                return Result;
            }

            public WeakFunc<T, string> GetFunc(WeakActionTestCase testCase)
            {
                WeakFunc<T, string> action = null;

                switch (testCase)
                {
                    case WeakActionTestCase.PublicNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPublicallyWithResult);
                        break;
                    case WeakActionTestCase.InternalNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffInternallyWithResult);
                        break;
                    case WeakActionTestCase.PrivateNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPrivatelyWithResult);
                        break;
                    case WeakActionTestCase.PublicStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPublicallyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.InternalStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffInternallyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.PrivateStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPrivatelyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.AnonymousStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            p =>
                            {
                                Result = Expected + p;
                                return Result;
                            });
                        break;
                    case WeakActionTestCase.AnonymousMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            p =>
                            {
                                Result = Expected + _index + p;
                                return Result;
                            });
                        break;
                }

                return action;
            }
        }

        internal class InternalNestedTestClass<T>
        {
            public const string Expected = "Hello";
            public const string Public = "Public";
            public const string Internal = "Internal";
            public const string InternalStatic = "InternalStatic";
            public const string Private = "Private";
            public const string PublicStatic = "PublicStatic";
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

            private string DoStuffPrivatelyWithResult(T parameter)
            {
                Result = Expected + Private + _index + parameter;
                return Result;
            }

            internal string DoStuffInternallyWithResult(T parameter)
            {
                Result = Expected + Internal + _index + parameter;
                return Result;
            }

            public string DoStuffPublicallyWithResult(T parameter)
            {
                Result = Expected + Public + _index + parameter;
                return Result;
            }

            private static string DoStuffPrivatelyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + PrivateStatic + parameter;
                return Result;
            }

            private static string DoStuffInternallyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + InternalStatic + parameter;
                return Result;
            }

            public static string DoStuffPublicallyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + PublicStatic + parameter;
                return Result;
            }

            public WeakFunc<T, string> GetFunc(WeakActionTestCase testCase)
            {
                WeakFunc<T, string> action = null;

                switch (testCase)
                {
                    case WeakActionTestCase.PublicNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPublicallyWithResult);
                        break;
                    case WeakActionTestCase.InternalNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffInternallyWithResult);
                        break;
                    case WeakActionTestCase.PrivateNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPrivatelyWithResult);
                        break;
                    case WeakActionTestCase.PublicStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPublicallyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.PrivateStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPrivatelyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.InternalStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffInternallyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.AnonymousStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            p =>
                            {
                                Result = Expected + p;
                                return Result;
                            });
                        break;
                    case WeakActionTestCase.AnonymousMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            p =>
                            {
                                Result = Expected + _index + p;
                                return Result;
                            });
                        break;
                }

                return action;
            }
        }

        private class PrivateNestedTestClass<T>
        {
            public const string Expected = "Hello";
            public const string Public = "Public";
            public const string Internal = "Internal";
            public const string InternalStatic = "InternalStatic";
            public const string Private = "Private";
            public const string PublicStatic = "PublicStatic";
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

            private string DoStuffPrivatelyWithResult(T parameter)
            {
                Result = Expected + Private + _index + parameter;
                return Result;
            }

            internal string DoStuffInternallyWithResult(T parameter)
            {
                Result = Expected + Internal + _index + parameter;
                return Result;
            }

            public string DoStuffPublicallyWithResult(T parameter)
            {
                Result = Expected + Public + _index + parameter;
                return Result;
            }

            private static string DoStuffPrivatelyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + PrivateStatic + parameter;
                return Result;
            }

            private static string DoStuffInternallyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + InternalStatic + parameter;
                return Result;
            }

            public static string DoStuffPublicallyAndStaticallyWithResult(T parameter)
            {
                Result = Expected + PublicStatic + parameter;
                return Result;
            }

            public WeakFunc<T, string> GetFunc(WeakActionTestCase testCase)
            {
                WeakFunc<T, string> action = null;

                switch (testCase)
                {
                    case WeakActionTestCase.PublicNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPublicallyWithResult);
                        break;
                    case WeakActionTestCase.InternalNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffInternallyWithResult);
                        break;
                    case WeakActionTestCase.PrivateNamedMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPrivatelyWithResult);
                        break;
                    case WeakActionTestCase.PublicStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPublicallyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.PrivateStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffPrivatelyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.InternalStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            DoStuffInternallyAndStaticallyWithResult);
                        break;
                    case WeakActionTestCase.AnonymousStaticMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            p =>
                            {
                                Result = Expected + p;
                                return Result;
                            });
                        break;
                    case WeakActionTestCase.AnonymousMethod:
                        action = new WeakFunc<T, string>(
                            this,
                            p =>
                            {
                                Result = Expected + _index + p;
                                return Result;
                            });
                        break;
                }

                return action;
            }
        }

        [Fact]
        public void TestInternalClassAnonymousMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestInternalClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + index + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + index + parameter,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassAnonymousStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + parameter,
                result);

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

            TestInternalClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.Internal + index + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.Internal + index + parameter,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassInternalStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.InternalStatic + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.InternalStatic + parameter,
                result);

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

            TestInternalClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.Private + index + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.Private + index + parameter,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPrivateStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.PrivateStatic + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.PrivateStatic + parameter,
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
            const string parameter = "My parameter";

            TestInternalClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.Public + index + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.Public + index + parameter,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestInternalClassPublicStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestInternalClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.PublicStatic + parameter,
                InternalNestedTestClass<string>.Result);
            Assert.Equal(
                InternalNestedTestClass<string>.Expected + InternalNestedTestClass<string>.PublicStatic + parameter,
                result);

            _itemInternal = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassAnonymousMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestPrivateClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + index + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + index + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassAnonymousStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPrivateClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassInternalNamedMethod()
        {
            Reset();

            const string parameter = "My parameter";
            const int index = 99;

            TestPrivateClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.Internal + index + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.Internal + index + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassInternalStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPrivateClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.InternalStatic + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.InternalStatic + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassPrivateNamedMethod()
        {
            Reset();

            const string parameter = "My parameter";
            const int index = 99;

            TestPrivateClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.Private + index + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.Private + index + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassPrivateStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPrivateClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.PrivateStatic + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.PrivateStatic + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassPublicNamedMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestPrivateClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.Public + index + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.Public + index + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPrivateClassPublicStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPrivateClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.PublicStatic + parameter,
                PrivateNestedTestClass<string>.Result);
            Assert.Equal(
                PrivateNestedTestClass<string>.Expected + PrivateNestedTestClass<string>.PublicStatic + parameter,
                result);

            _itemPrivate = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousMethod()
        {
            Reset();

            const int index = 99;
            const string parameter = "My parameter";

            TestPublicClassSetup(WeakActionTestCase.AnonymousMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + index + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + index + parameter,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassAnonymousStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassSetup(WeakActionTestCase.AnonymousStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + parameter,
                result);

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

            TestPublicClassSetup(WeakActionTestCase.InternalNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.Internal + index + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.Internal + index + parameter,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassInternalStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassSetup(WeakActionTestCase.InternalStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.InternalStatic + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.InternalStatic + parameter,
                result);

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

            TestPublicClassSetup(WeakActionTestCase.PrivateNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.Private + index + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.Private + index + parameter,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPrivateStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassSetup(WeakActionTestCase.PrivateStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.PrivateStatic + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.PrivateStatic + parameter,
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
            const string parameter = "My parameter";

            TestPublicClassSetup(WeakActionTestCase.PublicNamedMethod, index);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.Public + index + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.Public + index + parameter,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }

        [Fact]
        public void TestPublicClassPublicStaticMethod()
        {
            Reset();

            const string parameter = "My parameter";

            TestPublicClassSetup(WeakActionTestCase.PublicStaticMethod);

            Assert.True(_reference.IsAlive);
            Assert.True(_action.IsAlive);

            var result = _action.Execute(parameter);

            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.PublicStatic + parameter,
                PublicNestedTestClass<string>.Result);
            Assert.Equal(
                PublicNestedTestClass<string>.Expected + PublicNestedTestClass<string>.PublicStatic + parameter,
                result);

            _itemPublic = null;
            GC.Collect();

            Assert.False(_reference.IsAlive);
        }
    }
}