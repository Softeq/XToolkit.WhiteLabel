// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Tests.Commands.Utils;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public static class AsyncCommandsFactory
    {
        public static AsyncCommand Create(
            Func<Task> func,
            Func<bool> canExecute = null,
            Action<Exception> onException = null)
        {
            return new AsyncCommand(func, canExecute, onException);
        }

        public static AsyncCommand<T> Create<T>(
            Func<T, Task> func,
            Func<T, bool> canExecute = null,
            Action<Exception> onException = null)
        {
            return new AsyncCommand<T>(func, canExecute, onException);
        }

        public static AsyncCommand WithGarbageCollectableExecuteTarget(Func<Task> execute)
        {
            var executeProvider = new TestExecuteAsyncProvider(execute);
            return new AsyncCommand(executeProvider.Execute);
        }

        public static AsyncCommand WithGarbageCollectableCanExecuteTarget(Func<Task> execute, Func<bool> canExecute)
        {
            var canExecuteProvider = new TestCanExecuteProvider(canExecute);
            return new AsyncCommand(execute, canExecuteProvider.CanExecute);
        }

        public static AsyncCommand<T> WithGarbageCollectableExecuteTarget<T>(Func<T, Task> execute)
        {
            var executeProvider = new TestExecuteAsyncProvider<T>(execute);
            return new AsyncCommand<T>(executeProvider.Execute);
        }

        public static AsyncCommand<T> WithGarbageCollectableCanExecuteTarget<T>(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            var canExecuteProvider = new TestCanExecuteProvider<T>(canExecute);
            return new AsyncCommand<T>(execute, canExecuteProvider.CanExecute);
        }
    }
}
