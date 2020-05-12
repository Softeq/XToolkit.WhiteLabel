// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Common.Tests.Commands.Utils
{
    internal sealed class TestCanExecuteProvider
    {
        private readonly Func<bool> _canExecuteDelegate;

        public TestCanExecuteProvider(Func<bool> canExecuteDelegate)
        {
            _canExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute()
        {
            return _canExecuteDelegate.Invoke();
        }
    }

    internal sealed class TestCanExecuteProvider<T>
    {
        private readonly Func<T, bool> _canExecuteDelegate;

        public TestCanExecuteProvider(Func<T, bool> canExecuteDelegate)
        {
            _canExecuteDelegate = canExecuteDelegate;
        }

        public bool CanExecute(T parameter)
        {
            return _canExecuteDelegate.Invoke(parameter);
        }
    }
}
