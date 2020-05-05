// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;

namespace Softeq.XToolkit.Common.Tests.Commands.Utils
{
    internal sealed class TestExecuteAsyncProvider
    {
        private readonly Func<Task> _executeDelegate;

        public TestExecuteAsyncProvider(Func<Task> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public Task Execute()
        {
            return _executeDelegate.Invoke();
        }
    }

    internal sealed class TestExecuteAsyncProvider<T>
    {
        private readonly Func<T, Task> _executeDelegate;

        public TestExecuteAsyncProvider(Func<T, Task> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public Task Execute(T parameter)
        {
            return _executeDelegate.Invoke(parameter);
        }
    }
}
