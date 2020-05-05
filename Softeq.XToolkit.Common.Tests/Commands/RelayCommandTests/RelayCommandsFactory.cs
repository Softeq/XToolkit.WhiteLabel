// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Tests.Commands.Utils;

namespace Softeq.XToolkit.Common.Tests.Commands.RelayCommandTests
{
    public static class RelayCommandsFactory
    {
        public static RelayCommand WithGarbageCollectableExecuteTarget(Action execute)
        {
            var executeProvider = new TestExecuteProvider(execute);
            return new RelayCommand(executeProvider.Execute);
        }

        public static RelayCommand WithGarbageCollectableCanExecuteTarget(Action execute, Func<bool> canExecute)
        {
            var canExecuteProvider = new TestCanExecuteProvider(canExecute);
            return new RelayCommand(execute, canExecuteProvider.CanExecute);
        }

        public static RelayCommand<T> WithGarbageCollectableExecuteTarget<T>(Action<T> execute)
        {
            var executeProvider = new TestExecuteProvider<T>(execute);
            return new RelayCommand<T>(executeProvider.Execute);
        }

        public static RelayCommand<T> WithGarbageCollectableCanExecuteTarget<T>(Action<T> execute, Func<T, bool> canExecute)
        {
            var canExecuteProvider = new TestCanExecuteProvider<T>(canExecute);
            return new RelayCommand<T>(execute, canExecuteProvider.CanExecute);
        }
    }
}
