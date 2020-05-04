// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public static class AsyncCommandsFactory
    {
        public static AsyncCommand CreateAsyncCommand(Func<Task> func, Func<bool> canExecute = null)
        {
            return new AsyncCommand(func, canExecute);
        }

        public static AsyncCommand CreateAsyncCommandWithParam(
            Func<object, Task> func,
            Func<object, bool> canExecute = null,
            Action<Exception> onException = null)
        {
            return new AsyncCommand(func, canExecute, onException);
        }

        public static AsyncCommand<T> CreateAsyncCommandGeneric<T>(
            Func<T, Task> func,
            Func<bool> canExecute = null)
        {
            return new AsyncCommand<T>(func, canExecute);
        }

        public static AsyncCommand<T> CreateAsyncCommandGenericWithParam<T>(
            Func<T, Task> func,
            Func<object, bool> canExecute = null,
            Action<Exception> onException = null)
        {
            return new AsyncCommand<T>(func, canExecute, onException);
        }
    }
}
