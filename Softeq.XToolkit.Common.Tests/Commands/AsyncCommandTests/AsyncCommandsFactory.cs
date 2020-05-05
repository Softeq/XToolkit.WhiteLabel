// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public static class AsyncCommandsFactory
    {
        public static AsyncCommand CreateAsyncCommand(
            Func<Task> func,
            Func<bool> canExecute = null,
            Action<Exception> onException = null)
        {
            return new AsyncCommand(func, canExecute, onException);
        }

        public static AsyncCommand<T> CreateAsyncCommandGeneric<T>(
            Func<T, Task> func,
            Func<T, bool> canExecute = null,
            Action<Exception> onException = null)
        {
            return new AsyncCommand<T>(func, canExecute, onException);
        }
    }
}
