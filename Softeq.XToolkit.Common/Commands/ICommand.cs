// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     Defines a command.
    /// </summary>
    /// <typeparam name="T">Type of parameter.</typeparam>
    public interface ICommand<in T> : ICommand
    {
        bool CanExecute(T parameter);

        void Execute(T parameter);
    }

    public interface IRaisableCanExecute
    {
        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
