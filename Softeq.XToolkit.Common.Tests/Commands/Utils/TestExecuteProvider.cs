// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Tests.Commands.Utils
{
    internal sealed class TestExecuteProvider
    {
        private readonly Action _executeDelegate;

        public TestExecuteProvider(Action executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public void Execute()
        {
            _executeDelegate.Invoke();
        }
    }

    internal sealed class TestExecuteProvider<T>
    {
        private readonly Action<T> _executeDelegate;

        public TestExecuteProvider(Action<T> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public void Execute(T parameter)
        {
            _executeDelegate.Invoke(parameter);
        }
    }
}
