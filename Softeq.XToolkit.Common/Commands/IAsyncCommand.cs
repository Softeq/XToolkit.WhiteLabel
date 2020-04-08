// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;

namespace Softeq.XToolkit.Common.Commands
{
    /// <summary>
    ///     An Async implementation of <see cref="ICommand"/> for Task.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        ///     Executes the Command as a Task.
        /// </summary>
        /// <returns>The Task to execute.</returns>
        Task ExecuteAsync();

        /// <summary>
        ///     Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }

    /// <summary>
    ///     An Async implementation of ICommand for Task.
    /// </summary>
    /// <typeparam name="T">Type of parameter.</typeparam>
    public interface IAsyncCommand<in T> : ICommand
    {
        /// <summary>
        ///     Executes the Command as a Task.
        /// </summary>
        /// <returns>The Task to execute.</returns>
        /// <param name="parameter">
        ///     Data used by the command.
        ///     If the command does not require data to be passed, this object can be set to null.
        /// </param>
        Task ExecuteAsync(T parameter);

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
