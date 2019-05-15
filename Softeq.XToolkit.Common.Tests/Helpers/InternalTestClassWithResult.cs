// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;

namespace Softeq.XToolkit.Tests.Core.Common.Helpers
{
    internal class InternalTestClassWithResult
    {
        public const string Expected = "Hello";
        public const string Public = "Public";
        public const string Internal = "Internal";
        public const string Private = "Private";
        public const string PublicStatic = "PublicStatic";
        public const string PrivateStatic = "PrivateStatic";
        private readonly int _index; // Just here to force instance methods

        public InternalTestClassWithResult()
        {
        }

        public InternalTestClassWithResult(int index)
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
}