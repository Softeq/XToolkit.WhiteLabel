﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.Tests.Core.Common.Helpers
{
    public class PublicTestClassWithResult
    {
        public const string Expected = "Hello";
        public const string Public = "Public";
        public const string Private = "Private";
        public const string PublicStatic = "PublicStatic";
        public const string PrivateStatic = "PrivateStatic";

        private readonly int _index; // Just here to force instance methods

        public PublicTestClassWithResult(WeakActionTestCase testCase, int index)
            : this(testCase)
        {
            _index = index;
        }

        public PublicTestClassWithResult(WeakActionTestCase testCase)
        {
            switch (testCase)
            {
                case WeakActionTestCase.PublicNamedMethod:
                    Func = new WeakFunc<string>(
                        this,
                        DoStuffPublically);
                    break;
                case WeakActionTestCase.PrivateNamedMethod:
                    Func = new WeakFunc<string>(
                        this,
                        DoStuffPrivately);
                    break;
                case WeakActionTestCase.PublicStaticMethod:
                    Func = new WeakFunc<string>(
                        this,
                        DoStuffPublicallyAndStatically);
                    break;
                case WeakActionTestCase.PrivateStaticMethod:
                    Func = new WeakFunc<string>(
                        this,
                        DoStuffPrivatelyAndStatically);
                    break;
                case WeakActionTestCase.AnonymousStaticMethod:
                    Func = new WeakFunc<string>(
                        this,
                        () =>
                        {
                            Result = Expected;
                            return Result;
                        });
                    break;
                case WeakActionTestCase.AnonymousMethod:
                    Func = new WeakFunc<string>(
                        this,
                        () =>
                        {
                            Result = Expected + _index;
                            return Result;
                        });
                    break;
            }
        }

        public WeakFunc<string> Func { get; }

        public static string Result { get; private set; }

        public string Execute()
        {
            if (Func != null)
            {
                return Func.Execute();
            }

            return string.Empty;
        }

        private string DoStuffPrivately()
        {
            Result = Expected + Private + _index;
            return Result;
        }

        public string DoStuffPublically()
        {
            Result = Expected + Public;
            return Result;
        }

        private static string DoStuffPrivatelyAndStatically()
        {
            Result = Expected + PrivateStatic;
            return Result;
        }

        public static string DoStuffPublicallyAndStatically()
        {
            Result = Expected + PublicStatic;
            return Result;
        }
    }
}