﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.Tests.Core.Common.Helpers
{
    public class PublicTestClass
    {
        public const string Expected = "Hello";
        public const string Public = "Public";
        public const string Internal = "Internal";
        public const string Private = "Private";
        public const string PublicStatic = "PublicStatic";
        public const string InternalStatic = "InternalStatic";
        public const string PrivateStatic = "PrivateStatic";
        private readonly int _index; // Just here to force instance methods

        public PublicTestClass()
        {
        }

        public PublicTestClass(int index)
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

        private string DoStuffPrivatelyWithResult()
        {
            Result = Expected + Private + _index;
            return Result;
        }

        internal string DoStuffInternallyWithResult()
        {
            Result = Expected + Internal + _index;
            return Result;
        }

        public string DoStuffPublicallyWithResult()
        {
            Result = Expected + Public + _index;
            return Result;
        }

        private static string DoStuffPrivatelyAndStaticallyWithResult()
        {
            Result = Expected + PrivateStatic;
            return Result;
        }

        public static string DoStuffPublicallyAndStaticallyWithResult()
        {
            Result = Expected + PublicStatic;
            return Result;
        }

        internal static string DoStuffInternallyAndStaticallyWithResult()
        {
            Result = Expected + InternalStatic;
            return Result;
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

        public WeakFunc<string> GetFunc(WeakActionTestCase testCase)
        {
            WeakFunc<string> func = null;

            switch (testCase)
            {
                case WeakActionTestCase.PublicNamedMethod:
                    func = new WeakFunc<string>(
                        this,
                        DoStuffPublicallyWithResult);
                    break;
                case WeakActionTestCase.InternalNamedMethod:
                    func = new WeakFunc<string>(
                        this,
                        DoStuffInternallyWithResult);
                    break;
                case WeakActionTestCase.PrivateNamedMethod:
                    func = new WeakFunc<string>(
                        this,
                        DoStuffPrivatelyWithResult);
                    break;
                case WeakActionTestCase.PublicStaticMethod:
                    func = new WeakFunc<string>(
                        this,
                        DoStuffPublicallyAndStaticallyWithResult);
                    break;
                case WeakActionTestCase.InternalStaticMethod:
                    func = new WeakFunc<string>(
                        this,
                        DoStuffInternallyAndStaticallyWithResult);
                    break;
                case WeakActionTestCase.PrivateStaticMethod:
                    func = new WeakFunc<string>(
                        this,
                        DoStuffPrivatelyAndStaticallyWithResult);
                    break;
                case WeakActionTestCase.AnonymousStaticMethod:
                    func = new WeakFunc<string>(
                        this,
                        () =>
                        {
                            Result = Expected;
                            return Result;
                        });
                    break;
                case WeakActionTestCase.AnonymousMethod:
                    func = new WeakFunc<string>(
                        this,
                        () =>
                        {
                            Result = Expected + _index;
                            return Result;
                        });
                    break;
            }

            return func;
        }
    }
}