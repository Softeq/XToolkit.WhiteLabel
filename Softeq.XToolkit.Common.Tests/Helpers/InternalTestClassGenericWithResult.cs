﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.Tests.Core.Common.Helpers
{
    internal class InternalTestClassWithResult<T>
    {
        public const string Expected = "Hello";
        public const string Public = "Public";
        public const string Private = "Private";
        public const string PublicStatic = "PublicStatic";
        public const string PrivateStatic = "PrivateStatic";
        private readonly int _index; // Just here to force instance methods
        private readonly T _parameter;

        public InternalTestClassWithResult(WeakActionTestCase testCase, int index, T parameter)
            : this(testCase, parameter)
        {
            _index = index;
        }

        public InternalTestClassWithResult(WeakActionTestCase testCase, T parameter)
        {
            _parameter = parameter;

            switch (testCase)
            {
                case WeakActionTestCase.PublicNamedMethod:
                    Func = new WeakFunc<T, string>(
                        this,
                        DoStuffPublically);
                    break;
                case WeakActionTestCase.PrivateNamedMethod:
                    Func = new WeakFunc<T, string>(
                        this,
                        DoStuffPrivately);
                    break;
                case WeakActionTestCase.PublicStaticMethod:
                    Func = new WeakFunc<T, string>(
                        this,
                        DoStuffPublicallyAndStatically);
                    break;
                case WeakActionTestCase.PrivateStaticMethod:
                    Func = new WeakFunc<T, string>(
                        this,
                        DoStuffPrivatelyAndStatically);
                    break;
                case WeakActionTestCase.AnonymousStaticMethod:
                    Func = new WeakFunc<T, string>(
                        this,
                        p => Result = Expected + p);
                    break;
                case WeakActionTestCase.AnonymousMethod:
                    Func = new WeakFunc<T, string>(
                        this,
                        p => Result = Expected + _index + p);
                    break;
            }
        }

        public static string Result { get; private set; }

        public WeakFunc<T, string> Func { get; }

        public string Execute()
        {
            if (Func != null)
            {
                return Func.Execute(_parameter);
            }

            return string.Empty;
        }

        private string DoStuffPrivately(T parameter)
        {
            Result = Expected + Private + _index + parameter;
            return Result;
        }

        public string DoStuffPublically(T parameter)
        {
            Result = Expected + Public + parameter;
            return Result;
        }

        private static string DoStuffPrivatelyAndStatically(T parameter)
        {
            Result = Expected + PrivateStatic + parameter;
            return Result;
        }

        public static string DoStuffPublicallyAndStatically(T parameter)
        {
            Result = Expected + PublicStatic + parameter;
            return Result;
        }
    }
}